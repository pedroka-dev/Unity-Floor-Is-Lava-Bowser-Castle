using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is 100% not the best way to do this, but it gets the job done and is very perfomatic.
//In other scenarios, consider using a spriter renderer instead
public class GlobalTextureAnimator : MonoBehaviour
{
    public float animationSecondsInterval = 0.25f;
    public Material materialToAnimate;
    public List<Texture> cyclingTextures;

    void Start()
    {
        StartCoroutine(ChangeValue());
    }

    private IEnumerator ChangeValue()
    {
        foreach (var texture in cyclingTextures) {
            yield return new WaitForSeconds(animationSecondsInterval);
            materialToAnimate.mainTexture = texture;
        }
        StartCoroutine(ChangeValue());
    }
}

