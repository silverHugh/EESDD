﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VISSIM_COMSERVERLib;
using EESDDTEST.VISSIM;
namespace EESDD.VISSIM
{
    class BJUTVissim
    {

        //系统所需控件
        private Vissim vissim;
        private Net net;
        private DataVehicle data;
        private bool over;
        SignalHead head;
        SignalController signalcontroller;
        SignalGroup signalgroup;


        //需要获取的变量
        private double speed;


        private int lane;

       

        

        //路灯1为红色，3为绿灯，4为黄灯
        private int signalState;


        //计时器
        private double count = 0;

        public double Count
        {
            get { return count; }
            set { count = value; }
        }

        public int SignalState
        {
            get { return signalState; }
            set { signalState = value; }
        }


        
        public double Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        public int Lane
        {
            get { return lane; }
            set { lane = value; }
        }
        public bool Over
        {
            get { return over; }
            set { over = value; }
        }
        public DataVehicle Data
        {
            get { return data; }
            set { data = value; }
        }
        private Vehicle vehicle;

        public Vehicle Vehicle
        {
            get { return vehicle; }
            set { vehicle = value; }
        }

       
       
        public Net Net
        {
            get { return net; }
            set { net = value; }
        }
        private Simulation simulation;

        public Simulation Simulation
        {
            get { return simulation; }
            set { simulation = value; }
        }
        public Vissim Vissim
        {
            get { return vissim; }
            set { vissim = value; }
        }
        public BJUTVissim(String path)
        {
            over = false;
            initNet(path);
        }

        void initNet(String path)
        {
            vissim = new Vissim();
            vissim.LoadNet(path);
            net = vissim.Net;
            simulation = vissim.Simulation;
            //signalcontroller = net.SignalControllers.GetSignalControllerByNumber(5);
            //signalgroup = signalcontroller.SignalGroups.GetSignalGroupByNumber(6);
            //head = signalgroup.SignalHeads.GetSignalHeadByNumber(9);
            check();
            while ((vehicle = getVehicle(3)) == null)
            {
                RunSingle();
            }
        }
        
        public Simulation getSimulation()
        {
            return vissim.Simulation;
        }
        public Vehicle getVehicle(int i)
        {
            Vehicles vehicles = net.Vehicles;
            return vehicles.GetVehicleByNumber(i);
        }
        public void RunSingle()
        {
            if (count < simulation.Period * simulation.Resolution)
            {
                simulation.RunSingleStep();
                count++;
            }
            else 
            {
                Stop();
                count = -1;
            }
            
        }
        public void Stop()
        {
            simulation.Stop();
            vissim.Exit();
        }
        public void Run()
        {
            //signalgroup.set_AttValue("State", 1);
            while (/*(vehicle = getVehicle(3)) != null && */count!=-1)
            {
                vehicle.set_AttValue("SPEED", speed);
                vehicle.set_AttValue("LANE", lane);
                vehicle.set_AttValue("DESIREDSPEED", -1000);
                Console.WriteLine(vehicle.get_AttValue("LENGTH"));
                RunSingle();
            }
            
        }
        public void check()
        {
            Evaluation eval = vissim.Evaluation;
            eval.set_AttValue("DELAY", true);
            eval.set_AttValue("QUEUECOUNTER", true);
            eval.set_AttValue("VEHICLERECORD", true);
            //eval.DelayEvaluation.set_AttValue("FILE", true);
        }
    }
}
