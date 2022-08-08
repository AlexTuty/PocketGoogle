using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PocketGoogle;

namespace PocketGoogleTest
{
    [TestFixture]
    public class Indexer_Tests
    {
        private Dictionary<int, string> dictionary = new Dictionary<int, string>() //данные(файлы) для индексера
            {
                { 0, "A B C" },
                { 1, "B C" },
                { 2, "A C A C" },
                { 3, "F, f ff" }
            };
        private readonly Indexer i = new Indexer();

        //тест 1 (одиночный) - добавить файлы в индексер
        [Test]
        [Order(00)]
        public void Add() //добавляем файлы в индексер
        {
            var actual = true;
            foreach (var d in dictionary) //проходим по всем файлам
                i.Add(d.Key, d.Value); //добавляем файлы в индексер
            Assert.AreEqual(true, actual);
        }

        //тест 2 (цикл тестов) - получить id файлов из индексера
        [TestCase("C", new int[3] { 0, 1, 2 })] //ищем "С"
        [TestCase("X", new int[0])] //ищем "Х"
        [Order(01)]
        public void GetIds(string word, int[] expected)
        {
            var actual = i.GetIds(word);
            Assert.AreEqual(expected.ToList(), actual);
        }

        //тест 3 (цикл тестов) - получить позиции слов из файлов из индексера (тут была неправильная сигнатура вызова метода GetPositions)
        [TestCase(0, "A", new int[1] { 0 })] //ищем "A" в 0 файле
        [TestCase(2, "A", new int[2] { 0, 4 })] //ищем "A" во 2 файле
        [TestCase(3, "f", new int[1] { 3 })]
        [TestCase(3, "F", new int[1] { 0 })]
        [Order(02)]
        public void GetPositions(int id, string word, int[] expected)
        {
            var actual = i.GetPositions(id, word);
            Assert.AreEqual(expected.ToList(), actual);
        }

        //тест 4 (цикл тестов) - удалить файл из индексера, затем попытаться получить файлы по слову
        [TestCase("A", 1, new int[2] { 0, 2 })] //удаляем файл 1, получаем файлы для слова "А". new int[2] { 0, 2 }
        [TestCase("A", 0, new int[1] { 2 })] //удаляем файл 0, получаем файлы для слова "А". new int[1] { 2 }
        [TestCase("A", 2, new int[0])] //удаляем файл 2, получаем файлы для слова "А".new int[0]
        [Order(03)]
        public void Remove(string word, int id, int[] expected)
        {
            i.Remove(id);
            var actual = i.GetIds(word);
            Assert.AreEqual(expected.ToList(), actual);
        }

        //тест 5 (цикл тестов) - добавить, удалить, добавить один и тот же файл в индексер, затем произвести поиск слова в этом файле
        [TestCase("B", 1, new int[1] { 0 })]
        [Order(04)]
        public void Add_Remove_Add_GetPosition_B(string word, int id, int[] expected)
        {
            i.Add(id, word);
            i.Remove(id);
            i.Add(id, word); //добавляем файл 1
            var actual = i.GetPositions(id, word); //ищем слово "В" в файле 1 - "B C"
            Assert.AreEqual(expected.ToList(), actual); //должны получить new int[1] { 0 }
        }

        //тест 6 (цикл тестов) - добавить, удалить, добавить один и тот же файл в индексер, затем произвести поиск слова в этом файле
        [TestCase("B", 1, new int[0])]
        [Order(05)]
        public void Add_Remove_Add_GetPosition_A(string word, int id, int[] expected)
        {
            i.Add(id, word);
            i.Remove(id);
            i.Add(id, word); //добавляем файл 1
            var actual = i.GetPositions(id, "A"); //ищем слово "A" в файле 1 - "B C"
            Assert.AreEqual(expected.ToList(), actual); //должны получить new int[0]
        }
    }
}
