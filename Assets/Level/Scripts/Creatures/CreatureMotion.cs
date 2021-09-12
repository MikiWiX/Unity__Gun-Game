using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface EnemyMotion : EnemyComponent
{
    public void scaleSpeed(float scale);

    public void setSpeedScale(float scale);

}

namespace Characters
{
    public class CreatureMotion : MonoBehaviour
    {
        // Unity Input
        public float gravity = 1;
        public float jump = 20;

        public float maxSpeedX = 10;
        public float maxSpeedY = 200;

        public float uphillCoefficient = 0.5f;

        public float baseSpeed = 50;
        public float runSpeed = 80;
        public float runDelay = 2;

        public float speedScale = 1;

        public float minGroundNormalY = 0.6f; // max ground angle

        public float testDistanceOffsetX = 0.2f; //distance we check to the right/left from player center
        public float testDistanceY = 0.8f; //total height we check from character base
        public float teleportMaxSpeed = 0.25f; // if speed is less than this, teleportation may occur

        float motionDirection = 0; // previously noted motion direction; -1 =left, 0 =none, 1 =right
        float motionDuration = 0; //continuous motion duration

        // internal variables
        protected Rigidbody2D body;
        protected BoxCollider2D boxCollider2d;
        protected SkipPlatforms platforms;

        ContactFilter2D contactFilter;
        RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
        RaycastHit2D[] hitBuffer2 = new RaycastHit2D[16];
        ContactPoint2D[] contacts = new ContactPoint2D[16];

        //others
        const float minMoveDistance = 0.001f;
        const float shellRadius = 0.02f;

        Vector2 groundNormal = new Vector2(0, 0);


        //controls
        bool IS_GROUNDED = false, COLLIDING = false, JUMP_START = false, JUMPED = false;
        protected float direction = 0;

        private float initMaxSpeed;
        private float initBaseSpeed;
        private float initRunSpeed;

        protected void OnEnable()
        {
            body = GetComponent<Rigidbody2D>();
            boxCollider2d = GetComponent<BoxCollider2D>();
            platforms = GetComponent<SkipPlatforms>();
            body.isKinematic = false;
            body.freezeRotation = true;
            initSpeed();
        }

        private void initSpeed()
        {
            initMaxSpeed = maxSpeedX;
            initBaseSpeed = baseSpeed;
            initRunSpeed = runSpeed;
        }
        private void updateSpeed()
        {
            maxSpeedX = initMaxSpeed * speedScale;
            baseSpeed = initBaseSpeed * speedScale;
            runSpeed = initRunSpeed * speedScale;
        }

        public void scaleSpeed(float scale)
        {
            speedScale *= scale;
            updateSpeed();
        }

        public void setSpeedScale(float scale)
        {
            speedScale = scale;
            updateSpeed();
        }

        protected virtual void OnDisable()
        {
            body.isKinematic = false;
        }


        // Start is called before the first frame update
        protected virtual void Start()
        {
            contactFilter.useTriggers = false; // dont use trigger collider
            contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer)); //get and set this object layer to collider
            contactFilter.useLayerMask = true; // filter only collisions from loaded layer

            body.gravityScale = 0;//disable unity gravity
            jump *= 100;
            gravity *= 9.8f;
        }

        public void updateMovementDirection(float localDirection) //function for updating move data by Input or AI (from outside)
        {
            direction = localDirection;
        }

        public void Jump() //function for updating move data by Input or AI (from outside)
        {
            JUMP_START = true;
        }

        public void DropDown()
        {
            if (platforms != null)
            {
                platforms.DropDown();
            }
        }

        protected virtual void FixedUpdate()
        {
            JUMPED = false; // flag reset, we have not jumped this frame yet
            COLLIDING = false; // flag reset, we are not colliding this frame
            move();
            JUMP_START = false; // we are not starting jump any more, if we were, response has been made in testMove()
        }

        float localMaxWalkSpeed;
        Vector2 prevVel = new Vector2();

        private void move()
        {
            Vector2[] averageNormals = getNormalsAvgMinMax(boxCollider2d); // get normals for movement directions

            // if you stay in place, rotate average vector -90 deg
            // if you go right, rotate 'min' vector the same way instead
            // and if you move left, rotate 'max' vector
            int index = direction == 0 ? 0 : direction < 0 ? 1 : 2;
            Vector2 groundSurface = new Vector2(averageNormals[index].y, -averageNormals[index].x); // get move force vector
            groundNormal = averageNormals[index];

            // TELEPORT IF STUCK AT STAIRS
            if(direction != 0)
            {
                Vector2 bodyBase = body.position + (Vector2.down * (boxCollider2d.size.y / 2)); //base body position

                float coordX = direction * ((boxCollider2d.size.x / 2) + testDistanceOffsetX); // x axis offset

                double angle = Vector2.Angle(groundSurface * direction, new Vector2(coordX, 0)); 
                angle = angle * Math.PI / 180; //angle between X axis and groundSurface

                float len = coordX / (float)Math.Cos(angle); // length of line along ground normal with the right X axis length
                Vector2 checkBottom = (groundSurface * len) + bodyBase; // bottom of test line (at the end of len * groundSurface)
                Vector2 checkTop = checkBottom + (testDistanceY * groundNormal); // top of the test line

                //Debug.DrawLine(checkTop, checkTop - (groundNormal * testDistanceY), Color.blue);

                float count = Physics2D.Raycast(checkTop, -groundNormal, contactFilter, hitBuffer, testDistanceY); // cast a ray
                for (int i=0; i<count; i++) // if hit something
                {
                    if(hitBuffer[i].normal.y >= minGroundNormalY) // if its normal is good enough 
                    {
                        //we have to check if there is enough height for the player and if there is no obstacle in our way

                        //player wall has to be at collision point, so where is player center?
                        //hit  point - Xoffset (along ground) - Yoffset (perpendicular to ground)
                        Vector2 newPlayerPos = hitBuffer[i].point - ( direction * groundSurface * ((boxCollider2d.size.x/2) - shellRadius) ) + (groundNormal * ((boxCollider2d.size.y / 2)));
                          
                        //heigth test
                        Vector2 lineBegin = hitBuffer[i].point + (groundNormal * shellRadius);
                        Vector2 lineEnd = hitBuffer[i].point + (groundNormal * boxCollider2d.size.y);

                        //Debug.DrawLine(lineBegin, lineEnd, Color.red, 1);

                        bool tp = true;

                        //check space after teleport - there should be no terrain in there
                        float count2 = Physics2D.Raycast(lineBegin, groundNormal, contactFilter, hitBuffer2, boxCollider2d.size.y);
                        if(count2 > 0) // if there is something, check if it is terrain
                        {
                            for(int j=0; j<count2; j++)
                            {
                                if (hitBuffer2[j].collider.gameObject.tag == "TERRAIN")
                                {
                                    tp = false; // if it is, then no teleport
                                    break;
                                }
                            }
                        }

                        if (tp) // now, if the place is suitable, check if no terrain is between player and landing point
                        {
                            Vector2 midLine = (lineBegin + lineEnd) / 2;
                            Vector2 bodyToMidLine = midLine - body.position;

                            //Debug.DrawLine(midLine, body.position, Color.black, 1);

                            count2 = Physics2D.Raycast(body.position, bodyToMidLine, contactFilter, hitBuffer2, bodyToMidLine.magnitude);
                            if (count2 > 0) // if there is something, check if it is terrain
                            {
                                for (int j = 0; j < count2; j++)
                                {
                                    if (hitBuffer2[j].collider.gameObject.tag == "TERRAIN")
                                    {
                                        tp = false; //if there is terrain on the way, no teleport
                                        break;
                                    }
                                }
                            }
                            
                        }

                        if (tp && body.velocity.magnitude < 0.25f) // actually teleport if velocity is low
                        {
                            body.position = newPlayerPos;
                            body.velocity = prevVel;
                        }
                    }
                }
            }

            // ADD FORCES (Gravity and Speed)
            body.AddForce(-averageNormals[index] * 10 * body.mass * gravity); //add gravity oppose to normal
            
            localMaxWalkSpeed = getMaxWalkSpeed(); // get walking speed
            // how much uphill do we go? in range 2 (downhill) to 0 (uphill)
            double maxSpeedEfficiency = Vector2.Dot(Vector2.down, groundSurface * direction); // between -1 and 1
            maxSpeedEfficiency = (maxSpeedEfficiency * uphillCoefficient) + 1;
            if(maxSpeedEfficiency < 0) { maxSpeedEfficiency = 0; } // linear function from 0, by 1 to infinity

            float maxSpeed = localMaxWalkSpeed * (float)maxSpeedEfficiency;

            body.AddForce(groundSurface * direction * maxSpeed); // add walking force

            if (JUMP_START && IS_GROUNDED) // add jump force (if needed)
            {
                IS_GROUNDED = false;
                JUMPED = true;
                body.AddForce(Vector2.up * jump);
            }

            body.velocity = vectorSpeedLimit(body.velocity, maxSpeedX, maxSpeedY); //limit speed

            // UPDATE PREV VELOCITY
            prevVel = body.velocity;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {

        }

        private void OnCollisionStay2D(Collision2D collision) // it is executed AFTER FixedUpdate
        {
            // get average normal of all collision points between 2 colliders
            Vector2 averageNormal = getAverageNormal(collision);

            if (averageNormal.y >= minGroundNormalY)//if it is suitable for walking
            {
                if(!JUMPED) //and if we have not jumped earlier this frame
                {
                    IS_GROUNDED = true; //we are on the ground
                    COLLIDING = true; //we are colliding
                }
            }
        }
        private void OnCollisionExit2D(Collision2D collision) // and this comes after OnCollisionStay
        {
            // if we stopped colliding with one collider
            if (!COLLIDING) // if we are not colliding with anything good for walk
            {
                IS_GROUNDED = false; // then we are in air
            }
            
        }

        Vector2[] localNormalTab = new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };
        Vector2 localVec2;
        private Vector2[] getNormalsAvgMinMax(Collider2D collider)
        {
            localNormalTab[0].Set(0, 0); // localNormals[0] = Avg
            localNormalTab[1].Set(-2, 0); // localNormals[1] = Max (while moving right) x > -1
            localNormalTab[2].Set(2, 0); // localNormals[2] = Min (while moving left) x < 1

            float count = collider.GetContacts(contacts);
            for (int i=0; i<count; i++)
            {
                if(contacts[i].normal.y >= minGroundNormalY)
                {
                    localNormalTab[0] += contacts[i].normal;

                    if (contacts[i].normal.x > localNormalTab[1].x) 
                    { 
                        localNormalTab[1] = contacts[i].normal; 
                    }
                    if (contacts[i].normal.x < localNormalTab[2].x) 
                    { 
                        localNormalTab[2] = contacts[i].normal; 
                    }
                }
            }

            // if average normal = 0 (no normals in collider), place vertical normal
            if (localNormalTab[0].x == 0 && localNormalTab[0].y == 0) { localNormalTab[0].Set(0, 1); } 
            else { localNormalTab[0].Normalize(); } // else normalize
            // if 'max' is still -2 (no normals in collider), use vertical normal
            if (localNormalTab[1].x == -2) { localNormalTab[1].Set(0, 1); }
            // same for 'min'
            if (localNormalTab[2].x == 2) { localNormalTab[2].Set(0, 1); }

            return localNormalTab;
        }
        private Vector2 getAverageNormal(Collision2D collision)
        {
            localVec2.Set(0, 0.01f);
            for (int i = 0; i < collision.contactCount; i++)
            {
                localVec2 += collision.GetContact(i).normal;
            }
            return localVec2.normalized;
        }

        private Vector2 vectorSpeedLimit(Vector2 vector, float speed)
        {
            float len = vector.magnitude;
            if (len > speed)
            {
                float ratio = speed / len;
                vector *= ratio;
            }
            return vector;
        }
        private Vector2 vectorSpeedLimit(Vector2 vector, float xLim, float yLim)
        {
            if (vector.x > xLim) { vector.x = xLim; }
            if (vector.y > yLim) { vector.y = yLim; }
            if (vector.x < -xLim) { vector.x = -xLim; }
            if (vector.y < -yLim) { vector.y = -yLim; }
            return vector;
        }

        private float getMaxWalkSpeed()
        {
            float currentMaxSpeed = 0;
            motionDuration += Time.fixedDeltaTime;

            if(direction > 0)
            {
                if (motionDirection == 1)
                {
                    currentMaxSpeed = motionDuration > runDelay ? runSpeed : baseSpeed;
                }
                else
                {
                    motionDirection = 1;
                    motionDuration = 0;
                    currentMaxSpeed = baseSpeed;
                }
            } else if (direction == 0)
            {
                motionDirection = 0;
                currentMaxSpeed = baseSpeed;
            } else // if (direction < 0)
            {
                if (motionDirection == -1)
                {
                    currentMaxSpeed = motionDuration > runDelay ? runSpeed : baseSpeed;
                }
                else
                {
                    motionDirection = -1;
                    motionDuration = 0;
                    currentMaxSpeed = baseSpeed;
                }
            }
            return currentMaxSpeed;
        }
    }

    static class Vector2Extension
    {
        public static Vector2 projectAt(this Vector2 point, Vector2 vector)
        {
            float division = Vector2.Dot(point, vector) / (vector.x * vector.x + vector.y * vector.y);
            point.x = division * vector.x;
            point.y = division * vector.y;
            return point;
        }

        public static Vector2 projectAtNormalized(this Vector2 point, Vector2 vector)
        {
            float dot = Vector2.Dot(point, vector);
            point.x = dot * vector.x;
            point.y = dot * vector.y;
            return point;
        }
    }

}
