using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
public class RotatingHighlight : MonoBehaviour {
    public RectTransform image1;
    public RectTransform image2;

    private void Update() {
        image1.Rotate(new Vector3(0, 0, 150) * Time.deltaTime);
        image2.Rotate(new Vector3(0, 0, 150) * Time.deltaTime);
    }
}
}