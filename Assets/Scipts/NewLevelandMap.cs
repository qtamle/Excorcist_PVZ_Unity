using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewLevelandMap : MonoBehaviour
{
    public string[] map1Levels = { "Map 1 " }; // Tên của các scene trong map 1
    public string[] map2Levels = { "Map 2" }; // Tên của các scene trong map 2

    private int currentMap = 1; // Map hiện tại (bắt đầu từ map 1)
    private int currentLevelIndex = 0; // Index của level hiện tại trong map

    public void LoadNextLevel()
    {
        if (currentMap == 1)
        {
            // Nếu đang ở map 1, kiểm tra nếu đã hoàn thành level cuối của map 1
            if (currentLevelIndex == map1Levels.Length - 1)
            {
                // Chuyển từ map 1 sang map 2, vào level đầu tiên của map 2
                currentMap = 2;
                currentLevelIndex = 0;
                LoadLevel(map2Levels[currentLevelIndex]);
            }
            else
            {
                // Chuyển sang level tiếp theo trong map 1
                currentLevelIndex++;
                LoadLevel(map1Levels[currentLevelIndex]);
            }
        }
        else if (currentMap == 2)
        {
            // Nếu đang ở map 2, kiểm tra nếu đã hoàn thành level cuối của map 2
            if (currentLevelIndex == map2Levels.Length - 1)
            {
                Debug.Log("Bạn đã hoàn thành tất cả các level trong cả hai map!");
                // Xử lý khi đã hoàn thành hết các level ở cả hai map
                // Ví dụ: Hiển thị thông báo hoàn thành game, kết thúc game, ...
            }
            else
            {
                // Chuyển sang level tiếp theo trong map 2
                currentLevelIndex++;
                LoadLevel(map2Levels[currentLevelIndex]);
            }
        }
    }

    private void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}



