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
        private Dictionary<int, string> dictionary = new Dictionary<int, string>() //������(�����) ��� ���������
            {
                { 0, "A B C" },
                { 1, "B C" },
                { 2, "A C A C" },
                { 3, "F, f ff" }
            };
        private readonly Indexer i = new Indexer();

        //���� 1 (���������) - �������� ����� � ��������
        [Test]
        [Order(00)]
        public void Add() //��������� ����� � ��������
        {
            var actual = true;
            foreach (var d in dictionary) //�������� �� ���� ������
                i.Add(d.Key, d.Value); //��������� ����� � ��������
            Assert.AreEqual(true, actual);
        }

        //���� 2 (���� ������) - �������� id ������ �� ���������
        [TestCase("C", new int[3] { 0, 1, 2 })] //���� "�"
        [TestCase("X", new int[0])] //���� "�"
        [Order(01)]
        public void GetIds(string word, int[] expected)
        {
            var actual = i.GetIds(word);
            Assert.AreEqual(expected.ToList(), actual);
        }

        //���� 3 (���� ������) - �������� ������� ���� �� ������ �� ��������� (��� ���� ������������ ��������� ������ ������ GetPositions)
        [TestCase(0, "A", new int[1] { 0 })] //���� "A" � 0 �����
        [TestCase(2, "A", new int[2] { 0, 4 })] //���� "A" �� 2 �����
        [TestCase(3, "f", new int[1] { 3 })]
        [TestCase(3, "F", new int[1] { 0 })]
        [Order(02)]
        public void GetPositions(int id, string word, int[] expected)
        {
            var actual = i.GetPositions(id, word);
            Assert.AreEqual(expected.ToList(), actual);
        }

        //���� 4 (���� ������) - ������� ���� �� ���������, ����� ���������� �������� ����� �� �����
        [TestCase("A", 1, new int[2] { 0, 2 })] //������� ���� 1, �������� ����� ��� ����� "�". new int[2] { 0, 2 }
        [TestCase("A", 0, new int[1] { 2 })] //������� ���� 0, �������� ����� ��� ����� "�". new int[1] { 2 }
        [TestCase("A", 2, new int[0])] //������� ���� 2, �������� ����� ��� ����� "�".new int[0]
        [Order(03)]
        public void Remove(string word, int id, int[] expected)
        {
            i.Remove(id);
            var actual = i.GetIds(word);
            Assert.AreEqual(expected.ToList(), actual);
        }

        //���� 5 (���� ������) - ��������, �������, �������� ���� � ��� �� ���� � ��������, ����� ���������� ����� ����� � ���� �����
        [TestCase("B", 1, new int[1] { 0 })]
        [Order(04)]
        public void Add_Remove_Add_GetPosition_B(string word, int id, int[] expected)
        {
            i.Add(id, word);
            i.Remove(id);
            i.Add(id, word); //��������� ���� 1
            var actual = i.GetPositions(id, word); //���� ����� "�" � ����� 1 - "B C"
            Assert.AreEqual(expected.ToList(), actual); //������ �������� new int[1] { 0 }
        }

        //���� 6 (���� ������) - ��������, �������, �������� ���� � ��� �� ���� � ��������, ����� ���������� ����� ����� � ���� �����
        [TestCase("B", 1, new int[0])]
        [Order(05)]
        public void Add_Remove_Add_GetPosition_A(string word, int id, int[] expected)
        {
            i.Add(id, word);
            i.Remove(id);
            i.Add(id, word); //��������� ���� 1
            var actual = i.GetPositions(id, "A"); //���� ����� "A" � ����� 1 - "B C"
            Assert.AreEqual(expected.ToList(), actual); //������ �������� new int[0]
        }
    }
}
