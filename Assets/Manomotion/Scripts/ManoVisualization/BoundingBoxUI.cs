using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BoundingBoxUI : MonoBehaviour
{
    [SerializeField]
    TextMesh top_left, width, height;
    public LineRenderer bound_line_renderer;
    private ManoUtils mano_utils;


    private void Start()
    {
        mano_utils = ManoUtils.Instance;
        bound_line_renderer.positionCount = 4;
    }

    float smoothingFactor = 0.8f;
    float topLeftX = 0;
    float topLeftY = 0;
    float bbWidth = 0;
    float bbHeight = 0;
    // Update is called once per frame
    public void UpdateInfo(BoundingBox bounding_box)
    {
        topLeftX = Mathf.Lerp(topLeftX, bounding_box.top_left.x, smoothingFactor);
        topLeftY = Mathf.Lerp(topLeftY, bounding_box.top_left.y, smoothingFactor);
        bbWidth = Mathf.Lerp(bbWidth, bounding_box.width, smoothingFactor);
        bbHeight = Mathf.Lerp(bbHeight, bounding_box.height, smoothingFactor);

        if (!mano_utils)
            mano_utils = ManoUtils.Instance;
        if (!bound_line_renderer)
            Debug.Log("bound_line_renderer: null" );
        
        bound_line_renderer.SetPosition(0, mano_utils.CalculateNewPosition(new Vector3(topLeftX, 1 - topLeftY, 1), 1));
        bound_line_renderer.SetPosition(1, mano_utils.CalculateNewPosition(new Vector3(topLeftX + bbWidth, 1 - topLeftY, 1), 1));
        bound_line_renderer.SetPosition(2, mano_utils.CalculateNewPosition(new Vector3(topLeftX + bbWidth, (1 - topLeftY - bbHeight), 1), 1));
        bound_line_renderer.SetPosition(3, mano_utils.CalculateNewPosition(new Vector3(topLeftX, (1 - topLeftY - bbHeight), 1), 1));

        top_left.gameObject.transform.position = mano_utils.CalculateNewPosition(new Vector3(topLeftX, 1.05f - topLeftY, 1), 1);
        top_left.text = "Top Left: " + "X: " + topLeftX.ToString("F2") + " Y: " + topLeftY.ToString("F2");

        height.transform.position = mano_utils.CalculateNewPosition(new Vector3(topLeftX + bbWidth + 0.025f, (1 - topLeftY - bbHeight / 2f), 1), 1);
        height.GetComponent<TextMesh>().text = "Height: " + bbHeight.ToString("F2");

        width.transform.position = mano_utils.CalculateNewPosition(new Vector3(topLeftX + bbWidth / 2f, (1 - topLeftY - bbHeight) - 0.025f, 1), 1);
        width.GetComponent<TextMesh>().text = "Width: " + bbWidth.ToString("F2");

        //pure data
        //bound_line_renderer.SetPosition(0, mano_utils.CalculateNewPosition(new Vector3(bounding_box.top_left.x, 1 - bounding_box.top_left.y, 1), 1));
        //bound_line_renderer.SetPosition(1, mano_utils.CalculateNewPosition(new Vector3(bounding_box.top_left.x + bounding_box.width, 1 - bounding_box.top_left.y, 1), 1));
        //bound_line_renderer.SetPosition(2, mano_utils.CalculateNewPosition(new Vector3(bounding_box.top_left.x + bounding_box.width, (1 - bounding_box.top_left.y - bounding_box.height), 1), 1));
        //bound_line_renderer.SetPosition(3, mano_utils.CalculateNewPosition(new Vector3(bounding_box.top_left.x, (1 - bounding_box.top_left.y - bounding_box.height), 1), 1));

        //top_left.gameObject.transform.position = mano_utils.CalculateNewPosition(new Vector3(bounding_box.top_left.x, 1.05f - bounding_box.top_left.y, 1), 1);
        //top_left.text = "Top Left: " + "X: " + bounding_box.top_left.x.ToString("F2") + " Y: " + bounding_box.top_left.y.ToString("F2");

        //height.transform.position = mano_utils.CalculateNewPosition(new Vector3(bounding_box.top_left.x + bounding_box.width + 0.025f, (1 - bounding_box.top_left.y - bounding_box.height / 2f), 1), 1);
        //height.GetComponent<TextMesh>().text = "Height: " + bounding_box.height.ToString("F2");

        //width.transform.position = mano_utils.CalculateNewPosition(new Vector3(bounding_box.top_left.x + bounding_box.width / 2f, (1 - bounding_box.top_left.y - bounding_box.height) - 0.025f, 1), 1);
        //width.GetComponent<TextMesh>().text = "Width: " + bounding_box.width.ToString("F2");

    }
}
