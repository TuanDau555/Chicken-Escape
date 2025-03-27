using UnityEngine;

public class BackGroundController : MonoBehaviour
{
    private float startPosX,startPosY, lengthOfBackGround; //startPos is middle of background 
    public GameObject cam;
    [SerializeField] private float parallaxEffectX; //Speed at which the background move relative to the camera 
    [SerializeField] private float parallaxEffectY;

    void Awake()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        lengthOfBackGround = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
        ParallaxBackGround();
    }

    void ParallaxBackGround()
    {
        //Caculate distance background move based on cam movement
        // 0 = move with cam, 1 = won't move, 0.5 = half
        float distanceToRenderX = cam.transform.position.x * parallaxEffectX;
        float distanceToRenderY = cam.transform.position.y * parallaxEffectY;
        float movement = cam.transform.position.x * (1 - parallaxEffectX);

        transform.position = new Vector2(startPosX + distanceToRenderX, transform.position.y);
        transform.position = new Vector2(transform.position.x, startPosY + distanceToRenderY);
        //if background reached to the end of its length adjust its position for infinite rolling
        if (movement > startPosX + lengthOfBackGround)
        {
            startPosX += lengthOfBackGround;
        }

        else if (movement < startPosX - lengthOfBackGround)
        {
            startPosX -= lengthOfBackGround;
        }
    }
}
