namespace ApkReverse.Core.Sevices
{
    public interface IDisplayService
    {
        /// <summary>
        /// Выводит сообщение на экран
        /// </summary>
        /// <param name="msg"></param>
        void WriteMsg(string msg);

        /// <summary>
        /// Отображает диалог с возможностью выбора одного из 2-х вариантов ответа
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="yesIsDefault"></param>
        /// <param name="yesMsg"></param>
        /// <param name="noMsg"></param>
        /// <returns></returns>
        bool YesNoDialog(string msg, bool yesIsDefault = true, string yesMsg = "yes", string noMsg = "no");
    }
}