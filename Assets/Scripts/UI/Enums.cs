
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
    ����,
    �����ٶ�,
    ������,
    �����˺�,
    �ƶ��ٶ�,
    ����ֵ,
    �ָ��ٶ�,
    ����,
    ����,
      
}
public static class Enums
{
    //" AttackSpeed"תΪ" Attack Speed"
    public static string FormatStatName(Stat stat)
    {
        string formated = "";
        string unformatedString = stat.ToString();
        if (unformatedString.Length <= 0)
            return "�Ƿ�״̬��";
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