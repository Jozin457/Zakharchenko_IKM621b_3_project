﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zakharchenko_IKM621b_3_project
{
    class MajorWork
    {
        // поля
        private string Data; //вхідні дані
        private string Result; // Поле результату
        // методи
        public void Write(string D)
        {
            this.Data = D; //метод запису данних в об'єкт
        }
        public string Read()
        {
            return this.Result;// метод відображення результату
        }
        public void Task() // метод реалізації програмного завдання
        {
            if (this.Data.Length > 5)
            {
                this.Result = Convert.ToString(true);
            }
            else
            {
                this.Result = Convert.ToString(false);
            }
        }
    }
}