
public enum GameState
{
    MENU,
    WEAPONSELECTION,
    GAME,
    GAMEOVER,
    STAGECOMPLETE,
    WAVETRANSITION,
    SHOP
}
public enum Stat
{
    攻击,
    攻击速度,
    暴击率,
    暴击伤害,
    移动速度,
    生命值,
    恢复速度,
    幸运,
    闪避,
      
}
public static class Enums
{
    //" AttackSpeed"转为" Attack Speed"
    public static string FormatStatName(Stat stat)
    {
        string formated = "";
        string unformatedString = stat.ToString();
        if (unformatedString.Length <= 0)
            return "非法状态名";
        formated += unformatedString[0];
        for (int i=1;i<unformatedString.Length;i++)
        {
            if (char.IsUpper(unformatedString[i]))
                formated += " ";
            formated += unformatedString[i];
        }
        return formated;
    }
}