using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Enemies : MonoBehaviour
{
    /// <summary>
    /// 敵の数
    /// </summary>
    private const int EnemyNum = 8;

    /// <summary>
    /// 敵リスト
    /// </summary>
    [SerializeField]
    private Enemy[] enemies_ = new Enemy[EnemyNum];

    /// <summary>
    /// プレイヤー
    /// </summary>
    [SerializeField]
    private Player player_ = null;

    /// <summary>
    /// ターゲット中の敵インデックス
    /// </summary>
    public int targetIndex { get; set; } = 0;

    /// <summary>
    /// ターゲット可能な敵リスト
    /// </summary>
    private Enemy[] targets_ = new Enemy[EnemyNum];

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
		// プレイヤーの向きを単位ベクトルで取得する
		// プレイヤーのワールド行列を掛け合わせて、プレイヤーの前方を取得するが	
		// 「プレイヤーの向きを表す単位ベクトル」の計算である為、同次座標ではないことに注意
		var normalZ = new Vector3(0, 0, 1);
		var playerForward = player_.worldMatrix * normalZ;

		// プレイヤーの視野角の Cos 値
		var playerViewCos = Mathf.Cos(player_.viewRadian);

		// ターゲット可能な敵の一覧を更新する
		for (int i = 0; i < enemies_.Length; ++i)
        {
			// プレイヤーから敵までの向きを単位ベクトルで取得する
			var playerToEnemy = (enemies_[i].transform.position - player_.transform.position).normalized;
			
			// 内積を取得する
			var dot = Vector3.Dot(playerForward, playerToEnemy);

			// 内積の結果がプレイヤーの視野角より大きい場合はターゲット出来る
			if (playerViewCos <= dot)
			{
				targets_[i] = enemies_[i];
			}
			else
			{
				// これまでターゲットになっていた敵のターゲット設定を解除する
				if(targetIndex == i)
				{
					targets_[targetIndex]?.SetTarget(false);
				}
				targets_[i] = null;
			}
        }

		// ターゲットを更新する
        if (Input.GetKeyDown(KeyCode.Z))
        {
            UpdateTarget();
        }
    }

    /// <summary>
    /// ターゲットのインデックスを取得する
    /// </summary>
    /// <returns>ターゲットしている敵のインデックス</returns>
    public void UpdateTarget()
    {
        // これまでターゲットになっていた敵のターゲット設定を解除する
        targets_[targetIndex]?.SetTarget(false);

        // 次のターゲットとなるインデックスを探す
        for (int index = ((targetIndex + 1) % targets_.Length); index != targetIndex; index = ((index + 1) % targets_.Length))
        {
            if (targets_[index])
            {
                targetIndex = index;
                break;
            }
        }

        // 次のターゲットになる敵にターゲット設定を行う
        targets_[targetIndex]?.SetTarget(true);
    }

}