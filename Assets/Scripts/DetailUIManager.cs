using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailUIManager : MonoBehaviour
{
    public GameObject DetailPanel;
    public Text DetailTitleText;
    public Text DetailContentText;

    void FixedUpdate()
    {
        CardController hoverdCard = HandManager.instance.hoveredCard;
        bool hoveringGrid = HandManager.instance.hoveredGrid != null;
        if (hoverdCard == null && !hoveringGrid)
        {
            DetailPanel.gameObject.SetActive(false);
        }
        else
        {
            DetailPanel.gameObject.SetActive(true);
            if (hoverdCard != null)
            {
                switch (hoverdCard.missionType)
                {
                    case MissionType.Normal:
                        if (hoverdCard.missionLeft < 0)
                        {
                            DetailTitleText.text = "<color=#86AFFF>普通任务</color>";
                            DetailContentText.text = "执行完成后会消失" +
                            "\n当前任务 <color=#86AFFF>" + hoverdCard.missionName + "</color>" +
                            "\n剩余任务量 " + hoverdCard.missionProgress.ToString() + "/" + hoverdCard.missionAmount.ToString();
                        }
                        else
                        {
                            DetailTitleText.text = "限时任务";
                            DetailContentText.text = "本回合不执行会消失" +
                            "\n当前任务 <color=#FFBB00>" + hoverdCard.missionName + "</color" +
                            "\n剩余任务量 " + hoverdCard.missionProgress.ToString() + "/" + hoverdCard.missionAmount.ToString();
                        }
                        break;
                    case MissionType.Continues:
                        {
                            DetailTitleText.text = "<color=#6EE56E>永续任务</color>";
                            DetailContentText.text = "执行完成后进度重置" +
                            "\n当前任务 <color=#6EE56E>" + hoverdCard.missionName + "</color>" +
                            "\n剩余任务量 " + hoverdCard.missionProgress.ToString() + "/" + hoverdCard.missionAmount.ToString();
                        }
                        break;
                    case MissionType.Emergency:
                        {
                            DetailTitleText.text = "<color=#FF3C3C>危机任务</color>";
                            DetailContentText.text = "本回合必须执行！" +
                            "\n执行期间不能取消" +
                            "\n当前任务 <color=#FF3C3C>" + hoverdCard.missionName + "</color>" +
                            "\n剩余任务量 " + hoverdCard.missionProgress.ToString() + "/" + hoverdCard.missionAmount.ToString();
                        }
                        break;
                }
            }
            else if (hoveringGrid)
            {
                DetailTitleText.text = "人口";
                DetailContentText.text = "将任务分配给人口\n点击\"下个回合\"\n可以推进任务进度";
            }

        }
    }
}
