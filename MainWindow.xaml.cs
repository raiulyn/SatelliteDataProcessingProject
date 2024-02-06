﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Generic;
using Galileo6;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace SatelliteDataProcessingProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
            Init_IntUpDowns();
        }

        //4.1 Create two data structures using the LinkedList<T> class from the C# Systems.Collections.Generic namespace.
        //The data must be of type “double”;
        //you are not permitted to use any additional classes, nodes, pointers or data structures (array, list, etc) in the implementation of this application.
        //The two LinkedLists of type double are to be declared as global within the “public partial class”.
        public LinkedList<double> List1 { get; set; }
        public LinkedList<double> List2 { get; set; }

        //4.2 Copy the Galileo.DLL file into the root directory of your solution folder and add the appropriate reference in the solution explorer.
        //Create a method called “LoadData” which will populate both LinkedLists.
        //Declare an instance of the Galileo library in the method and create the appropriate loop construct to populate the two LinkedList; the data from Sensor A will populate the first LinkedList, while the data from Sensor B will populate the second LinkedList.
        //The LinkedList size will be hardcoded inside the method and must be equal to 400. The input parameters are empty, and the return type is void.
        public void LoadData()
        {
            List1 = new LinkedList<double>();
            List2 = new LinkedList<double>();
            var data = new Galileo6.ReadData();

            for (int i = 0; i < 400; i++)
            {
                List1.AddFirst(data.SensorA(Sigma_IntUpDown.Value.Value, Mu_IntUpDown.Value.Value)); //TODO: Replace values
                List2.AddFirst(data.SensorB(Sigma_IntUpDown.Value.Value, Mu_IntUpDown.Value.Value));
            }
        }
        private bool CheckIfCensorDataIsEmpty()
        {
            if(List1 != null && List2 != null)
            {
                return false;
            }
            return true;
        }

        //4.3 Create a custom method called “ShowAllSensorData” which will display both LinkedLists in a ListView.
        //Add column titles “Sensor A” and “Sensor B” to the ListView. The input parameters are empty, and the return type is void.
        public void ShowAllSensorData()
        {
            Sensors_ListView.Items.Clear();
            for (int i = 0; i < List1.Count; i++)
            {
                Sensors_ListView.Items.Add(new SensorDataItem { SensorA = List1.ElementAt(i).ToString(), SensorB = List2.ElementAt(i).ToString() });
            }
        }
        private class SensorDataItem
        {
            public string? SensorA { get; set; }
            public string? SensorB { get; set; }
        }


        //4.4 Create a button and associated click method that will call the LoadData and ShowAllSensorData methods.
        //The input parameters are empty, and the return type is void.
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
            ShowAllSensorData();
        }

        //4.5 Create a method called “NumberOfNodes” that will return an integer which is the number of nodes(elements) in a LinkedList.
        //The method signature will have an input parameter of type LinkedList, and the calling code argument is the linkedlist name.
        public int NumberOfNodes(LinkedList<double> list)
        {
            return list.Count;
        }

        //4.6 Create a method called “DisplayListboxData” that will display the content of a LinkedList inside the appropriate ListBox.
        //The method signature will have two input parameters; a LinkedList, and the ListBox name.
        //The calling code argument is the linkedlist name and the listbox name.
        public void DisplayListboxData(LinkedList<double> list, string listboxname)
        {
            switch (listboxname)
            {
                case "SensorA":
                    SensorA_ListBox.Items.Clear();
                    foreach (var item in list)
                    {
                        SensorA_ListBox.Items.Add(item);
                    }
                    break;
                case "SensorB":
                    SensorB_ListBox.Items.Clear();
                    foreach (var item in list)
                    {
                        SensorB_ListBox.Items.Add(item);
                    }
                    break;
                default:
                    break;
            }
        }

        //4.7 Create a method called “SelectionSort” which has a single input parameter of type LinkedList, while the calling code argument is the linkedlist name.
        //The method code must follow the pseudo code supplied below in the Appendix.
        //The return type is Boolean.
        public bool SelectionSort(LinkedList<double> list)
        {
            int min = 0;
            int max = NumberOfNodes(list);
            for (int i = 0; i < max; i++)
            {
                min = i;
                for (int j = i + 1; j < max; j++)
                {
                    if(list.ElementAt(j) < list.ElementAt(min))
                    {
                        min = j;
                    }
                }
                LinkedListNode<double> currentMin = list.Find(list.ElementAt(min));
                LinkedListNode<double> currentI = list.Find(list.ElementAt(i));

                var temp = currentMin.Value;
                currentMin.Value = currentI.Value;
                currentI.Value = temp;
            }
            
            return true;
        }

        //4.8 Create a method called “InsertionSort” which has a single parameter of type LinkedList, while the calling code argument is the linkedlist name.
        //The method code must follow the pseudo code supplied below in the Appendix. The return type is Boolean.
        public bool InsertionSort(LinkedList<double> list)
        {
            int max = NumberOfNodes(list);
            for (int i = 0; i < max - 1; i++)
            {
                for (int j = i + 1; j > 0; j--)
                {
                    if (list.ElementAt(j - 1) > list.ElementAt(j))
                    {
                        LinkedListNode<double> current = list.Find(list.ElementAt(j));

                        double tempvar = current.Previous.Value;
                        current.Previous.Value = current.Value;
                        current.Value = tempvar;
                    }
                }
            }
            return true;
        }

        //4.9 Create a method called “BinarySearchIterative” which has the following four parameters: LinkedList, SearchValue, Minimum and Maximum.
        //This method will return an integer of the linkedlist element from a successful search or the nearest neighbour value.
        //The calling code argument is the linkedlist name, search value, minimum list size and the number of nodes in the list.
        //The method code must follow the pseudo code supplied below in the Appendix.
        public int BinarySearchIterative(LinkedList<double> list, int searchValue, int min, int max)
        {
            while(min <= max - 1)
            {
                int middle = min + max / 2;
                if(searchValue == list.ElementAt(middle))
                {
                    return ++middle;
                }
                else if(searchValue < list.ElementAt(middle))
                {
                    max = middle - 1;
                }
                else
                {
                    max = middle + 1;
                }
            }
            return min;
        }

        //4.10 Create a method called “BinarySearchRecursive” which has the following four parameters: LinkedList, SearchValue, Minimum and Maximum.
        //This method will return an integer of the linkedlist element from a successful search or the nearest neighbour value.
        //The calling code argument is the linkedlist name, search value, minimum list size and the number of nodes in the list.
        //The method code must follow the pseudo code supplied below in the Appendix.
        public int BinarySearchRecursive(LinkedList<double> list, int searchValue, int min, int max)
        {
            if(min <= max - 1)
            {
                int middle = min + max / 2;
                if(searchValue == list.ElementAt(middle))
                {
                    return middle;
                }
                else if(searchValue < list.ElementAt(middle))
                {
                    return BinarySearchRecursive(list, searchValue, min, middle - 1);
                }
                else
                {
                    return BinarySearchRecursive(list, searchValue, middle + 1, max);
                }
            }
            return min;
        }

        /*
        * 4.11 Create button click methods that will search the LinkedList for an integer value entered into a textbox on the form. The four methods are:
        Method for Sensor A and Binary Search Iterative
        Method for Sensor A and Binary Search Recursive
        Method for Sensor B and Binary Search Iterative
        Method for Sensor B and Binary Search Recursive
        The search code must check to ensure the data is sorted, then start a stopwatch before calling the search method. 
        Once the search is complete the stopwatch will stop, and the number of ticks will be displayed in a read only textbox. 
        Finally, the code/method will call the “DisplayListboxData” method and highlight the search target number and two values on each side.
        */
        private void BinarySearchIterativeA_Button_Click(object sender, RoutedEventArgs e)
        {
            if (CheckIfCensorDataIsEmpty()) {  return; }

            SelectionSort(List1);

            var sw = Stopwatch.StartNew();
            
            BinarySearchIterative(List1, int.Parse(SearchTargetA_Textbox.Text), 0, List1.Count);

            sw.Stop();
            BinarySearchIterativeA_Timer.Text = sw.ElapsedTicks.ToString() + " ticks";

            DisplayListboxData(List1, "SensorA");
        }
        public void BinarySearchRecursiveA_Button_Click(object sender, RoutedEventArgs e)
        {
            if (CheckIfCensorDataIsEmpty()) { return; }

            int result = BinarySearchRecursive(List1, int.Parse(SearchTargetA_Textbox.Text), 0, List1.Count);
            Debug.WriteLine(result);
            DisplayListboxData(List1, "SensorA");
        }
        public void BinarySearchIterativeB_Button_Click(object sender, RoutedEventArgs e)
        {
            if (CheckIfCensorDataIsEmpty()) { return; }

            DisplayListboxData(List2, "SensorB");
        }
        public void BinarySearchRecursiveB_Button_Click(object sender, RoutedEventArgs e)
        {
            if (CheckIfCensorDataIsEmpty()) { return; }

            DisplayListboxData(List2, "SensorB");
        }

        /*
        * 4.12 Create button click methods that will sort the LinkedList using the Selection and Insertion methods. The four methods are:
        Method for Sensor A and Selection Sort
        Method for Sensor A and Insertion Sort
        Method for Sensor B and Selection Sort
        Method for Sensor B and Insertion Sort
        The button method must start a stopwatch before calling the sort method. 
        Once the sort is complete the stopwatch will stop, and the number of milliseconds will be displayed in a read only textbox. 
        Finally, the code/method will call the “ShowAllSensorData” method and “DisplayListboxData” for the appropriate sensor.
        */
        public void SelectionSortA_Button_Click(object sender, RoutedEventArgs e)
        {
            if (CheckIfCensorDataIsEmpty()) { return; }

            var sw = Stopwatch.StartNew();

            if (SelectionSort(List1))
            {
                ShowAllSensorData();
                DisplayListboxData(List1, "SensorA");
            }

            sw.Stop();
            SelectionSortA_Timer.Text = sw.ElapsedMilliseconds.ToString() + " milliseconds";
        }
        public void InsertionSortA_Button_Click(object sender, RoutedEventArgs e)
        {
            if (CheckIfCensorDataIsEmpty()) { return; }

            var sw = Stopwatch.StartNew();

            if (InsertionSort(List1))
            {
                ShowAllSensorData();
                DisplayListboxData(List1, "SensorA");
            }

            sw.Stop();
            InsertionSortA_Timer.Text = sw.ElapsedMilliseconds.ToString() + " milliseconds";
        }
        public void SelectionSortB_Button_Click(object sender, RoutedEventArgs e)
        {
            if (CheckIfCensorDataIsEmpty()) { return; }

            var sw = Stopwatch.StartNew();

            if (SelectionSort(List2))
            {
                ShowAllSensorData();
                DisplayListboxData(List2, "SensorB");
            }

            sw.Stop();
            SelectionSortB_Timer.Text = sw.ElapsedMilliseconds.ToString() + " milliseconds";
        }
        public void InsertionSortB_Button_Click(object sender, RoutedEventArgs e)
        {
            if (CheckIfCensorDataIsEmpty()) { return; }

            var sw = Stopwatch.StartNew();

            if (InsertionSort(List2))
            {
                ShowAllSensorData();
                DisplayListboxData(List2, "SensorB");
            }

            sw.Stop();
            InsertionSortB_Timer.Text = sw.ElapsedMilliseconds.ToString() + " milliseconds";
        }

        //4.13 Add numeric input controls for Sigma and Mu. The value for Sigma must be limited with a minimum of 10 and a maximum of 20.
        //Set the default value to 10. The value for Mu must be limited with a minimum of 35 and a maximum of 75. Set the default value to 50.
        public void Init_IntUpDowns()
        {
            Sigma_IntUpDown.Minimum = 10;
            Sigma_IntUpDown.Maximum = 20;
            Sigma_IntUpDown.DefaultValue = Sigma_IntUpDown.Value = 10;
            Mu_IntUpDown.Minimum = 35;
            Mu_IntUpDown.Maximum = 75;
            Mu_IntUpDown.DefaultValue = Mu_IntUpDown.Value = 50;
        }

        //4.14 Add textboxes for the search value; one for each sensor, ensure only numeric integer values can be entered.
        private void SearchTarget_Textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }

    //4.15 All code is required to be adequately commented.
    //Map the programming criteria and features to your code/methods by adding comments/regions above the method signatures.
    //Ensure your code is compliant with the CITEMS coding standards (refer http://www.citems.com.au/).
}