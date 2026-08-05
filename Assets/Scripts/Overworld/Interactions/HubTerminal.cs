using UnityEngine;

public class HubTerminal : InteractionBase
{
    public ChangeScene sceneChanger;
    public override void Interaction()
    {
        sceneChanger.MainScenePlay();
    }
}
