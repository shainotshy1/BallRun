using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkinSelectionHandler : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    public static List<GameObject> skinTypes = new List<GameObject>();

    [SerializeField] float centerDampingRange;
    [SerializeField] float centerXVal;
    [SerializeField] float centerSkinScale;
    [SerializeField] float normalSkinScale;
    [SerializeField] float scrollingSlowDoneSpeed;
    [SerializeField] float movementScaleFactor;
    [SerializeField] float recenterSpeed;
    [SerializeField] float skinSeperationDistance;
    [SerializeField] float skinInitialYPos;
    [SerializeField] List<GameObject> skins;
    [SerializeField] Transform skinParent;

    List<Transform> skinTransforms;
    float skinTransformDelta;
    bool isSlowing;
    bool isDragging;
    bool dragEnabled;
    bool centered;

    private void Start()
    {
        skinTransforms = new List<Transform>();
        skinTypes = skins;
        skinTransformDelta = 0;
        isDragging = false;
        centered = false;
        dragEnabled = true;
        InitializeSkin();
    }

    private void InitializeSkin()
    {
        if (PlayerPrefs.GetInt("SelectedSkinIndex") >= skins.Count)
        {
            PlayerPrefs.SetInt("SelectedSkinIndex", 0);
        }

        int center = PlayerPrefs.GetInt("SelectedSkinIndex");
        if (center < skins.Count)
        {
            GameObject selectedSkin = Instantiate(skins[center], new Vector3(centerXVal, skinInitialYPos, 0), Quaternion.identity, skinParent);
            skinTransforms.Add(selectedSkin.transform);
        }
        for (int i = center - 1; i >= 0; i--)
        {
            GameObject newSkin = Instantiate(skins[i], new Vector3(skinTransforms[0].position.x - skinSeperationDistance, skinInitialYPos, 0), Quaternion.identity, skinParent);
            skinTransforms.Insert(0, newSkin.transform);
        }
        for (int i = center + 1; i < skins.Count; i++)
        {
            GameObject newSkin = Instantiate(skins[i], new Vector3(skinTransforms[skinTransforms.Count - 1].position.x + skinSeperationDistance, skinInitialYPos, 0), Quaternion.identity, skinParent);
            skinTransforms.Add(newSkin.transform);
        }
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

        for(int i = 0; i < skinTransforms.Count; i++)
        {
            Transform skin = skinTransforms[i];
            float xDelta = Mathf.Abs(skin.position.x - centerXVal);
            if(xDelta < minXDelta)
            {
                minXDelta = xDelta;
                skinIndex = i;
            }
        }
        PlayerPrefs.SetInt("SelectedSkinIndex", skinIndex);
        PlayerPrefs.Save();
        return skinIndex;
    }
    private void Update()
    {
        UpdateSkinSizeOnScreen();
        UpdateSelectedSkin();
    }

    private void UpdateSkinSizeOnScreen()
    {
        for (int i = 0; i < skinTransforms.Count; i++)
        {
            float distanceToCenter = Mathf.Abs(centerXVal - skinTransforms[i].position.x);
            float interpolateValue = (distanceToCenter > centerDampingRange) ? 0 : 1 - distanceToCenter / centerDampingRange;

            Vector3 normalScale = normalSkinScale * Vector3.one;
            Vector3 targetScaleIncrease = centerSkinScale * Vector3.one - normalScale;

            skinTransforms[i].localScale = normalScale + interpolateValue * targetScaleIncrease;
        }
    }

    private void UpdateSelectedSkin()
    {
        int selectedSkin = IdentifyCenterSkin();
        if (!isDragging && skinTransforms.Count > 0 && !centered)
        {
            if (isSlowing)
            {
                int sign = CalculateSign(skinTransformDelta);

                skinTransformDelta -= sign * scrollingSlowDoneSpeed * Time.deltaTime;

                if (CalculateSign(skinTransformDelta) != sign || Mathf.Abs(skinTransformDelta) <= Mathf.Epsilon)
                {
                    skinTransformDelta = 0;
                    isSlowing = false;
                }
                ScrollSkins();
            }
            else if (selectedSkin >= 0 && selectedSkin < skins.Count)
            {
                isSlowing = false;
                int sign = CalculateSign(centerXVal - skinTransforms[selectedSkin].position.x);
                skinTransformDelta = sign * recenterSpeed * Time.deltaTime;

                ScrollSkins();

                if (CalculateSign(centerXVal - skinTransforms[selectedSkin].position.x) != sign)
                {
                    skinTransformDelta = centerXVal - skinTransforms[selectedSkin].position.x;
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
        foreach (Transform skin in skinTransforms)
        {
            skin.position = new Vector3(skin.position.x + skinTransformDelta, skin.position.y, skin.position.z);
        }
        if (skinTransforms.Count > 0)
        {
            if (Mathf.Abs(skinTransforms[IdentifyCenterSkin()].position.x - centerXVal) >  centerDampingRange)
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
