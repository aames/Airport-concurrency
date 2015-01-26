using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace AirportSim
{
    public partial class Airport : Form
    {
        private Buffer _buffer1InLanding;
        private Buffer _runwayBufferIn2TaxiBufferOut;
        private Buffer _runwayBufferOutTaxi1In;
        private Buffer _taxi1OutTerm1InBuffer;
        private Buffer _taxi1OutTerm1OutTaxi2In1Buffer;

        private Buffer _taxi2OutTerm2InBuffer;
        private Buffer _taxi2OutTerm2OutTaxi3InBuffer;

        private Buffer _taxi3OutTerm3InBuffer;
        private Buffer _taxi3OutTerm3OutTaxi4InBuffer;

        private Buffer _taxi4OutTaxi5InBuffer;
        private Buffer _taxi5OutRunwayInBuffer;


        private Semaphore _semaphore1InLanding;
        private Semaphore _runwaySemaphoreIn2TaxiSemaphoreOut;
        private Semaphore _runwaySemaphoreOutTaxi1In;
        private Semaphore _taxi1OutTerm1InSemaphore;
        private Semaphore _taxi1OutTerm1OutTaxi2In1Semaphore;
        private Semaphore _taxi2OutTerm2InSemaphore;
        private Semaphore _taxi2OutTerm2OutTaxi3InSemaphore;

        private Semaphore _taxi3OutTerm3InSemaphore;
        private Semaphore _taxi3OutTerm3OutTaxi4InSemaphore;
        private Semaphore _taxi4OutTaxi5InSemaphore;
        private Semaphore _taxi5OutRunwayInSemaphore;


        private RunwaySection _runway;

        private TerminalSection _terminal1;
        private TerminalSection _terminal2;
        private TerminalSection _terminal3;
        
        private TaxiSection _taxiSection1;
        private TaxiSection _taxiSection2;
        private TaxiSection _taxiSection3;
        private TaxiSection _taxiSection4;
        private TaxiSection _taxiSection5;
        private ArrivalSection _arrivalSection;

        private readonly List<Thread> _threads;

        public Airport()
        {
            InitializeComponent();
            _threads = new List<Thread>();
            SetupSections();
        }
        public void SetupSections()
        {
            _buffer1InLanding = new Buffer();
            _runwayBufferIn2TaxiBufferOut = new Buffer();
            _runwayBufferOutTaxi1In = new Buffer();

            _taxi1OutTerm1InBuffer = new Buffer();
            _taxi1OutTerm1OutTaxi2In1Buffer = new Buffer();

            _taxi2OutTerm2InBuffer = new Buffer();
            _taxi2OutTerm2OutTaxi3InBuffer = new Buffer();

            _taxi3OutTerm3InBuffer = new Buffer();
            _taxi3OutTerm3OutTaxi4InBuffer = new Buffer();

            _taxi4OutTaxi5InBuffer = new Buffer();
            _taxi5OutRunwayInBuffer = new Buffer();

            _semaphore1InLanding = new Semaphore();
            _runwaySemaphoreIn2TaxiSemaphoreOut = new Semaphore();
            _runwaySemaphoreOutTaxi1In = new Semaphore();

            _taxi1OutTerm1InSemaphore = new Semaphore();
            _taxi1OutTerm1OutTaxi2In1Semaphore = new Semaphore();

            _taxi2OutTerm2InSemaphore = new Semaphore();
            _taxi2OutTerm2OutTaxi3InSemaphore = new Semaphore();

            _taxi3OutTerm3InSemaphore = new Semaphore();
            _taxi3OutTerm3OutTaxi4InSemaphore = new Semaphore();

            _taxi4OutTaxi5InSemaphore = new Semaphore();
            _taxi5OutRunwayInSemaphore = new Semaphore();

           _runway = new RunwaySection(panel1)
                         {
                             Buffer1InLand = _buffer1InLanding,
                             Buffer2In = _taxi5OutRunwayInBuffer,
                             Buffer3Out = _runwayBufferOutTaxi1In,
                             Semaphore1InLand = _semaphore1InLanding,
                             Semaphore2In = _taxi5OutRunwayInSemaphore,
                             Semaphore3Out = _runwaySemaphoreOutTaxi1In,
                             Angle = 270,
                             TotalLanded = label4,
                             TotalTakeOffs = label5
                         };

            _terminal1 = new TerminalSection(panel3)
                             {
                                 BufferIn = _taxi1OutTerm1InBuffer,
                                 SemaphoreIn = _taxi1OutTerm1InSemaphore,
                                 BufferOut = _taxi1OutTerm1OutTaxi2In1Buffer,
                                 SemaphoreOut = _taxi1OutTerm1OutTaxi2In1Semaphore,
                                 Angle = 0,
                                 Hold = false
                             };
            _terminal2 = new TerminalSection(panel6)
                             {
                                 BufferIn = _taxi2OutTerm2InBuffer,
                                 BufferOut = _taxi2OutTerm2OutTaxi3InBuffer,
                                 SemaphoreIn = _taxi2OutTerm2InSemaphore,
                                 SemaphoreOut = _taxi2OutTerm2OutTaxi3InSemaphore,
                                 Angle = 0,
                                 Hold = false
                             };
            _terminal3 = new TerminalSection(panel5)
                             {
                                 BufferIn = _taxi3OutTerm3InBuffer,
                                 BufferOut = _taxi3OutTerm3OutTaxi4InBuffer,
                                 SemaphoreIn = _taxi3OutTerm3InSemaphore,
                                 SemaphoreOut = _taxi3OutTerm3OutTaxi4InSemaphore,
                                 Angle = 0,
                                 Hold = false
                             };
            
            _taxiSection1 = new TaxiSection(panel2)
                                {
                                    Buffer1In = _runwayBufferOutTaxi1In,
                                    Semaphore1In = _runwaySemaphoreOutTaxi1In,
                                    Buffer1OutToTerm = _taxi1OutTerm1InBuffer,
                                    Semaphore1OutToTerm = _taxi1OutTerm1InSemaphore,
                                    Buffer2OutToTaxi = _taxi1OutTerm1OutTaxi2In1Buffer,
                                    Semaphore2OutToTaxi = _taxi1OutTerm1OutTaxi2In1Semaphore,
                                    Buffer1OutDestination = 1,
                                    Angle = 0
                                };
            _taxiSection2 = new TaxiSection(panel7)
                                {
                                    Buffer1In = _taxi1OutTerm1OutTaxi2In1Buffer,
                                    Buffer1OutToTerm = _taxi2OutTerm2InBuffer,
                                    Buffer2OutToTaxi = _taxi2OutTerm2OutTaxi3InBuffer,
                                    Semaphore1In = _taxi1OutTerm1OutTaxi2In1Semaphore,
                                    Semaphore1OutToTerm = _taxi2OutTerm2InSemaphore,
                                    Semaphore2OutToTaxi = _taxi2OutTerm2OutTaxi3InSemaphore,
                                    Buffer1OutDestination = 2,
                                    Angle = 90
                                };
            _taxiSection3 = new TaxiSection(panel9)
                                 {
                                    Buffer1In = _taxi2OutTerm2OutTaxi3InBuffer,
                                    Buffer1OutToTerm = _taxi3OutTerm3InBuffer,
                                    Buffer2OutToTaxi = _taxi3OutTerm3OutTaxi4InBuffer,
                                    Semaphore1In = _taxi2OutTerm2OutTaxi3InSemaphore,
                                    Semaphore1OutToTerm = _taxi3OutTerm3InSemaphore,
                                    Semaphore2OutToTaxi = _taxi3OutTerm3OutTaxi4InSemaphore,
                                    Buffer1OutDestination = 3,
                                    Angle = 90
                                };
            _taxiSection4 = new TaxiSection(panel10)
                                {
                                    Buffer1In = _taxi3OutTerm3OutTaxi4InBuffer,
                                    Buffer2OutToTaxi = _taxi4OutTaxi5InBuffer,
                                    Semaphore1In = _taxi3OutTerm3OutTaxi4InSemaphore,
                                    Semaphore2OutToTaxi = _taxi4OutTaxi5InSemaphore,
                                    Buffer1OutDestination = 4,
                                    Angle = 90
                                };
            _taxiSection5 = new TaxiSection(panel4)
                                {
                                    Buffer1In = _taxi4OutTaxi5InBuffer,
                                    Buffer2OutToTaxi = _taxi5OutRunwayInBuffer,
                                    Semaphore1In = _taxi4OutTaxi5InSemaphore,
                                    Semaphore2OutToTaxi = _taxi5OutRunwayInSemaphore,
                                    Buffer1OutDestination = 4,
                                    Angle = 180
                                };
            _arrivalSection = new ArrivalSection(panel11)
                                  {
                                      BufferOut = _buffer1InLanding,
                                      SemaphoreOut = _semaphore1InLanding,
                                      Land = false,
                                      Sleep = (int)numericUpDown1.Value,
                                      RadioBtn1 = rbTerm1,
                                      RadioBtn2 = radioButton2,
                                      RadioBtn3 = radioButton3,
                                      RadioBtn4 = radioButton4
                                  };
            var t = new Thread(_runway.Run);
            t.Start();
            _threads.Add(t);
            t = new Thread(_taxiSection1.Run);
            t.Start();
            _threads.Add(t);
            t = new Thread(_terminal1.Run);
            t.Start();
            _threads.Add(t);
            t = new Thread(_terminal2.Run);
            t.Start();
            _threads.Add(t);
            t = new Thread(_terminal3.Run);
            t.Start();
            _threads.Add(t);
            t = new Thread(_taxiSection2.Run);
            t.Start();
            _threads.Add(t);
            t = new Thread(_taxiSection3.Run);
            t.Start();
            _threads.Add(t);
            t = new Thread(_taxiSection4.Run);
            t.Start();
            _threads.Add(t);
            t = new Thread(_taxiSection5.Run);
            t.Start();
            _threads.Add(t);
            t = new Thread(_arrivalSection.Run);
            t.Start();
            _threads.Add(t);
            t = new Thread(_arrivalSection.AddPlaneToQueue);
            t.Start();
            _threads.Add(t);
            
        }
       
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }


        private void Land_Click(object sender, EventArgs e)
        {
            Land.BackColor = _arrivalSection.Land ? Color.Red : Color.LawnGreen;
            _arrivalSection.Land = !_arrivalSection.Land;
            _runway.Land = _arrivalSection.Land;
        }

        private void btnTerm1_Click(object sender, EventArgs e)
        {
            _terminal1.Hold = !_terminal1.Hold;
            btnTerm1.BackColor = _terminal1.Hold ? Color.Red : Color.LawnGreen;
        }

        private void btnTerm2_Click(object sender, EventArgs e)
        {
            _terminal2.Hold = !_terminal2.Hold;
            btnTerm2.BackColor = _terminal2.Hold ? Color.Red : Color.LawnGreen;
        }

        private void btnTerm3_Click(object sender, EventArgs e)
        {
            _terminal3.Hold = !_terminal3.Hold;
            btnTerm3.BackColor = _terminal3.Hold ? Color.Red : Color.LawnGreen;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            _arrivalSection.Sleep = (int)numericUpDown1.Value;
        }

        
    }
}
