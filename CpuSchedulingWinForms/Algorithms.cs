using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpuSchedulingWinForms
{
    public static class Algorithms
    {
        public static void srtfAlgorithm(string userInput)
        {
			int np = Convert.ToInt16(userInput); // # processes
            double[] arrivalTime = new double[np];
            double[] burstTime = new double[np];
            double[] remainingTime = new double[np];
            double[] turnAroundTime = new double[np];
            double[] waitTime = new double[np];
            bool[] finished = new bool[np];

            double activeTime = 0;
            double completedTime = 0;
            int completedProcesses = 0;

			DialogResult result = MessageBox.Show("Shortest Remaining Time First Scheduling ", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                try
                {
                    for (int i = 0; i < np; i++)
                    {
                        string input =
                        Microsoft.VisualBasic.Interaction.InputBox("Enter Arrival time: ",
                                                           "Arrival time for P" + (i + 1),
                                                           "",
                                                           -1, -1);
                        arrivalTime[i] = Convert.ToInt64(input);
                    }

                    // Get Burst time for each process
                    for (int i = 0; i < np; i++)
                    {
                        string input =
                        Microsoft.VisualBasic.Interaction.InputBox("Enter Burst time: ",
                                                           "Burst time for P" + (i + 1),
                                                           "",
                                                           -1, -1);
                        burstTime[i] = Convert.ToInt64(input);

                    }

                    // Designate Remaining Time 
                    for (int i = 0; i < np; i++)
                    {
                        remainingTime[i] = burstTime[i];
                    }

                    
                    while (completedProcesses != np)
                    {
                        double minRemainingTime = int.MaxValue;
                        int currentProcess = int.MinValue;
                        for (int i = 0; i < np; i++)
                        {
                            if (arrivalTime[i] <= completedTime && !finished[i] && remainingTime[i] < minRemainingTime)
                            {
                                minRemainingTime = remainingTime[i];
                                currentProcess = i;
                            }
                            
                        }

                        completedTime++;
                        if (currentProcess != int.MinValue)
                        {
							activeTime++;
							remainingTime[currentProcess] -= 1;
                            if (remainingTime[currentProcess] == 0)
                            {
                                finished[currentProcess] = true;
                                turnAroundTime[currentProcess] = completedTime - arrivalTime[currentProcess];
                                waitTime[currentProcess] = turnAroundTime[currentProcess] - burstTime[currentProcess];
                                completedProcesses++;
                            }
                        }

                    }

                    double avgTurnAroundTime = 0;
                    foreach (double time in turnAroundTime) { avgTurnAroundTime += time; }
                    avgTurnAroundTime /= np;

                    double avgWaitTime = 0;
                    foreach (double wait in waitTime) { avgWaitTime += wait; }
                    avgWaitTime /= np;

                    for (int i = 0; i < np; i++)
                    {
                        MessageBox.Show("Turnaround time for P" + (i + 1) + " = " + turnAroundTime[i], "Wait", MessageBoxButtons.OK, MessageBoxIcon.None);
                        MessageBox.Show("Waiting time for P" + (i + 1) + " = " + waitTime[i], "Wait", MessageBoxButtons.OK, MessageBoxIcon.None);

                    }

                    MessageBox.Show("Average turnaround time for " + np + " processes" + " = " + avgTurnAroundTime + " sec(s)", "Average Awaiting Time", MessageBoxButtons.OK, MessageBoxIcon.None);
                    MessageBox.Show("Average waiting time for " + np + " processes" + " = " + avgWaitTime + " sec(s)", "Average Awaiting Time", MessageBoxButtons.OK, MessageBoxIcon.None);
                    MessageBox.Show("CPU Utilization time for " + np + " processes" + " = " + (activeTime / completedTime) + " sec(s)", "CPU Utilization Time", MessageBoxButtons.OK, MessageBoxIcon.None);
                
                }

                catch (Exception e)
                {

                }
			}

		}

		public static void hrrnAlgorithm(string userInput)
        {
            int np = int.Parse(userInput);
			double[] arrivalTime = new double[np];
			double[] burstTime = new double[np];
            double[] respRatio = new double[np];
			bool[] finished = new bool[np];

			double[] turnAroundTime = new double[np];
			double[] waitTime = new double[np];
            double[] timeCompleted = new double[np];


            bool[] isActive = new bool[np]; // used to measure the active CPU time
            bool hasArrived = false; // processing algorithm will be skipped and time incremented if none of the arrival times reached
            

			double activeTime = 0;
			double completedTime = 0;
			int completedProcesses = 0;

			DialogResult result = MessageBox.Show("Highest Response Ratio Next Scheduling ", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                for (int i = 0; i < np; i++)
                {
                    // Assign each processes' arrival and burst times
					string input =
					Microsoft.VisualBasic.Interaction.InputBox("Enter Arrival time: ",
													   "Arrival time for P" + (i + 1),
													   "",
													   -1, -1);
					arrivalTime[i] = Convert.ToInt64(input);

					input =
					Microsoft.VisualBasic.Interaction.InputBox("Enter Burst time: ",
													   "Burst time for P" + (i + 1),
													   "",
													   -1, -1);
					burstTime[i] = Convert.ToInt64(input);
				}
                
                while (completedProcesses != np)
                {
                  
                    for (int i = 0; i < np; i++)
                    {
                        if (arrivalTime[i] <= completedTime && !finished[i])
                        {
                            int wait = Convert.ToInt32(completedTime - arrivalTime[i]);
                            respRatio[i] = (int)(wait + burstTime[i]) / burstTime[i];
                            hasArrived = true;
                            isActive[i] = true;
                        }
                    }

                    if (!hasArrived) { completedTime++; continue; }
                    

                    int highestRRprocess = int.MinValue;
                    int maxRatio = int.MinValue;
                    for (int i = 0; i < np; i++)
                    {
                        if (respRatio[i] > maxRatio && isActive[i])
                        {
                            maxRatio = (int)respRatio[i];
                            highestRRprocess = i;
                        }
                    }


					for (int i = 0; i < np; i++)
					{
						if (isActive[i])
						{
							hasArrived = true;
							break;
						}
						hasArrived = false;
					}



                    if (!isActive[highestRRprocess]) { completedTime++; continue; }


                    activeTime+= (int)burstTime[highestRRprocess];
                    completedTime += (int)burstTime[highestRRprocess]; // Realized here I should have just made the arrays int
                    timeCompleted[highestRRprocess] = completedTime;
                    finished[highestRRprocess] = true;
                    isActive[highestRRprocess] = false;
                    completedProcesses++;

				}

                double avgTurnAround = 0;
                double avgWaitTime = 0;
                for (int i = 0; i < np; i++)
                {
                    turnAroundTime[i] = timeCompleted[i] - arrivalTime[i];
                    waitTime[i] = turnAroundTime[i] - burstTime[i];

                    avgTurnAround += turnAroundTime[i];
                    avgWaitTime += waitTime[i];
					MessageBox.Show("Completion time for P" + (i + 1) + " = " + timeCompleted[i], "Wait", MessageBoxButtons.OK, MessageBoxIcon.None);

					MessageBox.Show("Turnaround time for P" + (i + 1) + " = " + turnAroundTime[i], "Wait", MessageBoxButtons.OK, MessageBoxIcon.None);
					MessageBox.Show("Waiting time for P" + (i + 1) + " = " + waitTime[i], "Wait", MessageBoxButtons.OK, MessageBoxIcon.None);
				}
                avgTurnAround /= np;
                avgWaitTime /= np;
                double cpuUtilTime = activeTime / completedTime;

				MessageBox.Show("Average Turnaround time for Processes" + (np) + " = " + avgTurnAround, "Avg TT", MessageBoxButtons.OK, MessageBoxIcon.None);
				MessageBox.Show("Average Waiting time for Processes" + (np) + " = " + avgWaitTime, "Avg Wait", MessageBoxButtons.OK, MessageBoxIcon.None);
				MessageBox.Show("CPU Utilization for Processes" + (np) + " = " + cpuUtilTime, "Avg Wait", MessageBoxButtons.OK, MessageBoxIcon.None);

			}
		}

		public static void fcfsAlgorithm(string userInput)
        {
            int np = Convert.ToInt16(userInput);
            int npX2 = np * 2;

            double[] bp = new double[np];
            double[] wtp = new double[np];
            string[] output1 = new string[npX2];
            double twt = 0.0, awt; 
            int num;

            DialogResult result = MessageBox.Show("First Come First Serve Scheduling ", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {

                // Gets the burst time for each process
                for (num = 0; num <= np - 1; num++)
                {
                    //MessageBox.Show("Enter Burst time for P" + (num + 1) + ":", "Burst time for Process", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    //Console.WriteLine("\nEnter Burst time for P" + (num + 1) + ":");

                    string input =
                    Microsoft.VisualBasic.Interaction.InputBox("Enter Burst time: ",
                                                       "Burst time for P" + (num + 1),
                                                       "",
                                                       -1, -1);

                    bp[num] = Convert.ToInt64(input);
                }

                for (num = 0; num <= np - 1; num++)
                {
                    if (num == 0)
                    {
                        wtp[num] = 0;
                    }
                    else
                    {
                        wtp[num] = wtp[num - 1] + bp[num - 1];
                        MessageBox.Show("Waiting time for P" + (num + 1) + " = " + wtp[num], "Job Queue", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }
                }
                for (num = 0; num <= np - 1; num++)
                {
                    twt = twt + wtp[num];
                }
                awt = twt / np;
                MessageBox.Show("Average waiting time for " + np + " processes" + " = " + awt + " sec(s)", "Average Awaiting Time", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            else if (result == DialogResult.No)
            {
                //this.Hide();
                //Form1 frm = new Form1();
                //frm.ShowDialog();
            }
        }

        public static void sjfAlgorithm(string userInput)
        {
            int np = Convert.ToInt16(userInput);

            double[] bp = new double[np];
            double[] wtp = new double[np];
            double[] p = new double[np];
            double twt = 0.0, awt; 
            int x, num;
            double temp = 0.0;
            bool found = false;

            DialogResult result = MessageBox.Show("Shortest Job First Scheduling", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                for (num = 0; num <= np - 1; num++)
                {
                    string input =
                        Microsoft.VisualBasic.Interaction.InputBox("Enter burst time: ",
                                                           "Burst time for P" + (num + 1),
                                                           "",
                                                           -1, -1);

                    bp[num] = Convert.ToInt64(input);
                }
                for (num = 0; num <= np - 1; num++)
                {
                    p[num] = bp[num];
                }
                for (x = 0; x <= np - 2; x++)
                {
                    for (num = 0; num <= np - 2; num++)
                    {
                        if (p[num] > p[num + 1])
                        {
                            temp = p[num];
                            p[num] = p[num + 1];
                            p[num + 1] = temp;
                        }
                    }
                }
                for (num = 0; num <= np - 1; num++)
                {
                    if (num == 0)
                    {
                        for (x = 0; x <= np - 1; x++)
                        {
                            if (p[num] == bp[x] && found == false)
                            {
                                wtp[num] = 0;
                                MessageBox.Show("Waiting time for P" + (x + 1) + " = " + wtp[num], "Waiting time:", MessageBoxButtons.OK, MessageBoxIcon.None);
                                //Console.WriteLine("\nWaiting time for P" + (x + 1) + " = " + wtp[num]);
                                bp[x] = 0;
                                found = true;
                            }
                        }
                        found = false;
                    }
                    else
                    {
                        for (x = 0; x <= np - 1; x++)
                        {
                            if (p[num] == bp[x] && found == false)
                            {
                                wtp[num] = wtp[num - 1] + p[num - 1];
                                MessageBox.Show("Waiting time for P" + (x + 1) + " = " + wtp[num], "Waiting time", MessageBoxButtons.OK, MessageBoxIcon.None);
                                //Console.WriteLine("\nWaiting time for P" + (x + 1) + " = " + wtp[num]);
                                bp[x] = 0;
                                found = true;
                            }
                        }
                        found = false;
                    }
                }
                for (num = 0; num <= np - 1; num++)
                {
                    twt = twt + wtp[num];
                }
                MessageBox.Show("Average waiting time for " + np + " processes" + " = " + (awt = twt / np) + " sec(s)", "Average waiting time", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public static void priorityAlgorithm(string userInput)
        {
            int np = Convert.ToInt16(userInput);

            DialogResult result = MessageBox.Show("Priority Scheduling ", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                double[] bp = new double[np];
                double[] wtp = new double[np + 1];
                int[] p = new int[np];
                int[] sp = new int[np];
                int x, num;
                double twt = 0.0;
                double awt;
                int temp = 0;
                bool found = false;
                for (num = 0; num <= np - 1; num++)
                {
                    string input =
                        Microsoft.VisualBasic.Interaction.InputBox("Enter burst time: ",
                                                           "Burst time for P" + (num + 1),
                                                           "",
                                                           -1, -1);

                    bp[num] = Convert.ToInt64(input);
                }
                for (num = 0; num <= np - 1; num++)
                {
                    string input2 =
                        Microsoft.VisualBasic.Interaction.InputBox("Enter priority: ",
                                                           "Priority for P" + (num + 1),
                                                           "",
                                                           -1, -1);

                    p[num] = Convert.ToInt16(input2);
                }
                for (num = 0; num <= np - 1; num++)
                {
                    sp[num] = p[num];
                }
                for (x = 0; x <= np - 2; x++)
                {
                    for (num = 0; num <= np - 2; num++)
                    {
                        if (sp[num] > sp[num + 1])
                        {
                            temp = sp[num];
                            sp[num] = sp[num + 1];
                            sp[num + 1] = temp;
                        }
                    }
                }
                for (num = 0; num <= np - 1; num++)
                {
                    if (num == 0)
                    {
                        for (x = 0; x <= np - 1; x++)
                        {
                            if (sp[num] == p[x] && found == false)
                            {
                                wtp[num] = 0;
                                MessageBox.Show("Waiting time for P" + (x + 1) + " = " + wtp[num], "Waiting time", MessageBoxButtons.OK);
                                //Console.WriteLine("\nWaiting time for P" + (x + 1) + " = " + wtp[num]);
                                temp = x;
                                p[x] = 0;
                                found = true;
                            }
                        }
                        found = false;
                    }
                    else
                    {
                        for (x = 0; x <= np - 1; x++)
                        {
                            if (sp[num] == p[x] && found == false)
                            {
                                wtp[num] = wtp[num - 1] + bp[temp];
                                MessageBox.Show("Waiting time for P" + (x + 1) + " = " + wtp[num], "Waiting time", MessageBoxButtons.OK);
                                //Console.WriteLine("\nWaiting time for P" + (x + 1) + " = " + wtp[num]);
                                temp = x;
                                p[x] = 0;
                                found = true;
                            }
                        }
                        found = false;
                    }
                }
                for (num = 0; num <= np - 1; num++)
                {
                    twt = twt + wtp[num];
                }
                MessageBox.Show("Average waiting time for " + np + " processes" + " = " + (awt = twt / np) + " sec(s)", "Average waiting time", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //Console.WriteLine("\n\nAverage waiting time: " + (awt = twt / np));
                //Console.ReadLine();
            }
            else
            {
                //this.Hide();
            }
        }

        public static void roundRobinAlgorithm(string userInput)
        {
            int np = Convert.ToInt16(userInput);
            int i, counter = 0;
            double total = 0.0;
            double timeQuantum;
            double waitTime = 0, turnaroundTime = 0;
            double averageWaitTime, averageTurnaroundTime;
            double[] arrivalTime = new double[10];
            double[] burstTime = new double[10];
            double[] temp = new double[10];
            int x = np;

            DialogResult result = MessageBox.Show("Round Robin Scheduling", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                for (i = 0; i < np; i++)
                {
                    string arrivalInput =
                            Microsoft.VisualBasic.Interaction.InputBox("Enter arrival time: ",
                                                               "Arrival time for P" + (i + 1),
                                                               "",
                                                               -1, -1);

                    arrivalTime[i] = Convert.ToInt64(arrivalInput);

                    string burstInput =
                            Microsoft.VisualBasic.Interaction.InputBox("Enter burst time: ",
                                                               "Burst time for P" + (i + 1),
                                                               "",
                                                               -1, -1);

                    burstTime[i] = Convert.ToInt64(burstInput);

                    temp[i] = burstTime[i];
                }
                string timeQuantumInput =
                            Microsoft.VisualBasic.Interaction.InputBox("Enter time quantum: ", "Time Quantum",
                                                               "",
                                                               -1, -1);

                timeQuantum = Convert.ToInt64(timeQuantumInput);
                Helper.QuantumTime = timeQuantumInput;

                for (total = 0, i = 0; x != 0;)
                {
                    if (temp[i] <= timeQuantum && temp[i] > 0)
                    {
                        total = total + temp[i];
                        temp[i] = 0;
                        counter = 1;
                    }
                    else if (temp[i] > 0)
                    {
                        temp[i] = temp[i] - timeQuantum;
                        total = total + timeQuantum;
                    }
                    if (temp[i] == 0 && counter == 1)
                    {
                        x--;
                        //printf("nProcess[%d]tt%dtt %dttt %d", i + 1, burst_time[i], total - arrival_time[i], total - arrival_time[i] - burst_time[i]);
                        MessageBox.Show("Turnaround time for Process " + (i + 1) + " : " + (total - arrivalTime[i]), "Turnaround time for Process " + (i + 1), MessageBoxButtons.OK);
                        MessageBox.Show("Wait time for Process " + (i + 1) + " : " + (total - arrivalTime[i] - burstTime[i]), "Wait time for Process " + (i + 1), MessageBoxButtons.OK);
                        turnaroundTime = (turnaroundTime + total - arrivalTime[i]);
                        waitTime = (waitTime + total - arrivalTime[i] - burstTime[i]);                        
                        counter = 0;
                    }
                    if (i == np - 1)
                    {
                        i = 0;
                    }
                    else if (arrivalTime[i + 1] <= total)
                    {
                        i++;
                    }
                    else
                    {
                        i = 0;
                    }
                }
                averageWaitTime = Convert.ToInt64(waitTime * 1.0 / np);
                averageTurnaroundTime = Convert.ToInt64(turnaroundTime * 1.0 / np);
                MessageBox.Show("Average wait time for " + np + " processes: " + averageWaitTime + " sec(s)", "", MessageBoxButtons.OK);
                MessageBox.Show("Average turnaround time for " + np + " processes: " + averageTurnaroundTime + " sec(s)", "", MessageBoxButtons.OK);
            }
        }
    }
}

