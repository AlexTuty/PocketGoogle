using System.Collections.Generic;

namespace PocketGoogle
{
    public interface IIndexer
    {
        /// <summary>
        ///     Adds the document to the index
        ///     Добавление документа в индекс
        /// </summary>
        /// <param name="id"></param>
        /// <param name="text"></param>
        void Add(int id, string documentText);

        /// <summary>
        ///     Removes document from the index
        ///     Удаляет документ из указателя
        /// </summary>
        /// <param name="id"></param>
        void Remove(int id);

        /// <summary>
        ///     Returns all the ids such that corresponding documents contains a word
        ///     Возвращает все идентификаторы, такие, что соответствующие документы содержат слово
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        List<int> GetIds(string word);

        /// <summary>
        ///     Returns all the positions in the document with the given id, where the given word starts
        ///     Возвращает все позиции в документе с заданным идентификатором, где начинается данное слово
        /// </summary>
        /// <param name="id"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        List<int> GetPositions(int id, string word);
    }
}