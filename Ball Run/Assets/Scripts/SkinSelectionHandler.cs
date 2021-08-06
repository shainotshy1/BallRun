using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkinSelectionHandler : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    [SerializeField] float centerDampingRange;
    [SerializeField] float centerXVal;
    [SerializeField] float centerSkinScale;
    [SerializeField] float normalSkinScale;
    [SerializeField] float scrollingSlowDoneSpeed;
    [SerializeField] float movementScaleFactor;
    [SerializeField] float recenterSpeed;
    [SerializeField] List<Transform> skins;


    float skinTransformDelta;
    bool isSlowing;
    bool isDragging;
    bool dragEnabled;
    bool centered;

    private void Start()
    {
        skinTransformDelta = 0;
        isDragging = false;
        centered = false;
        dragEnabled = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if(dragEnabled) skinTransformDelta = eventData.delta.x / movementScaleFactor;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = dragEnabled;
        centered = false;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        isSlowing = true;
    }
    private int IdentifyCenterSkin()
    {
        int skinIndex = -1;
        float minXDelta = Mathf.Infinity;

        for(int i = 0; i < skins.Count; i++)
        {
            Transform skin = skins[i];
            float xDelta = Mathf.Abs(skin.position.x - centerXVal);
            if(xDelta < minXDelta)
            {
                minXDelta = xDelta;
                skinIndex = i;
            }
        }

        return skinIndex;
    }
    private void Update()
    {
        int selectedSkin = IdentifyCenterSkin();
  
        for(int i = 0; i < skins.Count; i++)
        {
            float distanceToCenter = Mathf.Abs(centerXVal - skins[i].position.x);
            float interpolateValue = (distanceToCenter > centerDampingRange) ? 0 : 1-distanceToCenter/centerDampingRange;

            Vector3 normalScale = normalSkinScale * Vector3.one;
            Vector3 targetScaleIncrease = centerSkinScale * Vector3.one - normalScale;

            skins[i].localScale = normalScale + interpolateValue * targetScaleIncrease;
        }
        if (!isDragging && skins.Count > 0 && !centered)
        {
            if (isSlowing)
            {
                int sign = CalculateSign(skinTransformDelta);

                skinTransformDelta -= sign * scrollingSlowDoneSpeed*Time.deltaTime;

                if (CalculateSign(skinTransformDelta) != sign||Mathf.Abs(skinTransformDelta)<=Mathf.Epsilon)
                {
                    skinTransformDelta = 0;
                    isSlowing = false;
                }
                ScrollSkins();
            }
            else
            {
                isSlowing = false;
                int sign = CalculateSign(centerXVal - skins[selectedSkin].position.x);
                skinTransformDelta = sign * recenterSpeed*Time.deltaTime;

                ScrollSkins();

                if (CalculateSign(centerXVal - skins[selectedSkin].position.x) != sign)
                {
                    skinTransformDelta = centerXVal - skins[selectedSkin].position.x;
                    centered = true;
                }
            }
        }
        else
        {
            ScrollSkins();
        }
    }

    private void ScrollSkins()
    {
        foreach (Transform skin in skins)
        {
            skin.position = new Vector3(skin.position.x + skinTransformDelta, skin.position.y, skin.position.z);
        }
        if (skins.Count > 0)
        {
            if (Mathf.Abs(skins[IdentifyCenterSkin()].position.x - centerXVal) >  centerDampingRange)
            {
                dragEnabled = false;
                skinTransformDelta = 0;
            }
            else
            {
                dragEnabled = true;
            }
        }
        if (!isSlowing&&!isDragging)
        {
            skinTransformDelta = 0;
        }
    }

    private int CalculateSign(float val)
    {
        if (val == 0) return 0;
        return (int)(Mathf.Abs(val) / val);
    }
}
