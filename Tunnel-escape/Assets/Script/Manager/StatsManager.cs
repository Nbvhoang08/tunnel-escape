using UnityEngine;

public class StatsManager : Singleton<StatsManager>
{
    private const string DefenseKey = "PlayerDefense";
    private const string CooldownReductionKey = "PlayerCooldownReduction";

    private const float DefaultDefense = 0; // Giá trị mặc định của Defense
    private const float DefaultCooldownReduction = 1f; // Giá trị mặc định của CooldownReduction

    private const float MaxDefense = 0.8f; // Giới hạn tối đa Defense (80%)

    // Lấy chỉ số phòng thủ (tính theo phần trăm)
    public float GetDefense()
    {
        return PlayerPrefs.GetFloat(DefenseKey, DefaultDefense);
    }

    // Lưu chỉ số phòng thủ (giới hạn trong khoảng 0% - 80%)
    public void SaveDefense(float defense)
    {
        PlayerPrefs.SetFloat(DefenseKey, Mathf.Clamp(defense, 0, MaxDefense)); // Giới hạn 0% - 80%
        PlayerPrefs.Save();
    }

    // Tăng chỉ số phòng thủ
    public void IncreaseDefense(float amount)
    {
        float currentDefense = GetDefense();
        SaveDefense(currentDefense + amount); // Tăng thêm `amount`
    }

    // Giảm chỉ số phòng thủ
    public void DecreaseDefense(int amount)
    {
        float currentDefense = GetDefense();
        SaveDefense(currentDefense - amount); // Giảm `amount`
    }

    // Lấy giảm hồi chiêu (phần trăm)
    public float GetCooldownReduction()
    {
        return PlayerPrefs.GetFloat(CooldownReductionKey, DefaultCooldownReduction);
    }

    // Lưu giảm hồi chiêu (giới hạn trong khoảng 0% - 100%)
    public void SaveCooldownReduction(float cooldownReduction)
    {
        PlayerPrefs.SetFloat(CooldownReductionKey, Mathf.Clamp(cooldownReduction, 0.1f, 1f));
        PlayerPrefs.Save();
    }

    // Tăng giảm hồi chiêu
    public void IncreaseCooldownReduction(float amount)
    {
        float currentCooldownReduction = GetCooldownReduction();
        SaveCooldownReduction(currentCooldownReduction - amount);
    }

    // Reset các chỉ số về mặc định
    public void ResetStats()
    {
        SaveDefense(DefaultDefense);
        SaveCooldownReduction(DefaultCooldownReduction);
    }
}
