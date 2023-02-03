using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerController : MonoBehaviour
{
    [SerializeField]
    private Player player = null;

    [SerializeField]
    private CameraThatFollowsATransform cameraThatFollowsATransform = null;

    private IEnumerator Start()
    {
        /*
        yield return new WaitForSeconds(5);
        cameraThatFollowsATransform.SetOrthosize(20);
        yield return new WaitForSeconds(5);
        cameraThatFollowsATransform.ReturnZoom();
        */
        yield return new WaitForSeconds(0.25f);
        cameraThatFollowsATransform.StartFollow(player.transform);
    }
}
