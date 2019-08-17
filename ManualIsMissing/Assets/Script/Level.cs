using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private const float CameraOrthoSize = 50f;
    private const float TamCastle=8f;
    private const float TamTopCastle = 4f;
    private void Start()
    {
        CreateCastle(40f, 20f,true);
        CreateCastle(40f, 20f, false);
    }
    private void CreateCastle(float height, float xPosition,bool createBottom)
    {
        //CastleTop
        Transform head = Instantiate(GameAssets.GetInstance().pfTopCastle);
        float CastleTopPosition;
        if(createBottom)
        {
            CastleTopPosition = -CameraOrthoSize + height - TamTopCastle;
        }
        else
        {
            CastleTopPosition = +CameraOrthoSize - height - TamTopCastle;
        }
        head.position = new Vector3(xPosition,CastleTopPosition);

        //CastleBody
        Transform body = Instantiate(GameAssets.GetInstance().pfCastleBody);
        float CastleBodyPosition;
        if(createBottom)
        {
            CastleBodyPosition = -CameraOrthoSize;
        }
        else
        {
            CastleBodyPosition = +CameraOrthoSize;
            body.localScale = new Vector3(1, -1, 1);
        }
        body.position = new Vector3(xPosition, CastleBodyPosition);

        SpriteRenderer CastleBodySprite = body.GetComponent<SpriteRenderer>();
        CastleBodySprite.size = new Vector2(TamCastle, height);

        BoxCollider2D boxcolliderbody = body.GetComponent<BoxCollider2D>();
        boxcolliderbody.size = new Vector2(TamCastle, height);
        boxcolliderbody.offset = new Vector2(0f, height*0.5f);
    }
}
