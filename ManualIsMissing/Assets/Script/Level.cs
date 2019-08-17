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
        CreateGapCAstles(25f, 50f, 20f);
    }

    private void CreateGapCAstles(float gapy, float gapSize, float xposition)
    {
        CreateCastle(gapy - gapSize * .5f, xposition, true);
        CreateCastle(CameraOrthoSize * 2f - gapy - gapSize * .5f, xposition, false);
    }
    private void CreateCastle(float height, float xPosition,bool createBottom)
    {
        //CastleTop
        Transform head = Instantiate(GameAssets.GetInstance().pfTopCastle);
        float CastleTopPosition;
        if(createBottom)
        {
            CastleTopPosition = -CameraOrthoSize + height - TamTopCastle*.5f;
        }
        else
        {
            CastleTopPosition = +CameraOrthoSize - height - TamTopCastle*.5f;
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
