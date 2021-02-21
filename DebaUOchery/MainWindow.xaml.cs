using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using System.Drawing;
using Rectangle = System.Drawing.Rectangle;
using Point = System.Drawing.Point;
using System.Drawing.Imaging;
using Color = System.Drawing.Color;
using System.Reflection;
using System.IO;

namespace DebaUOchery
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const UInt32 FLASHW_STOP = 0; //Stop flashing. The system restores the window to its original state.        private const UInt32 FLASHW_CAPTION = 1; //Flash the window caption.        
        private const UInt32 FLASHW_TRAY = 2; //Flash the taskbar button.        
        private const UInt32 FLASHW_ALL = 3; //Flash both the window caption and taskbar button.        
        private const UInt32 FLASHW_TIMER = 4; //Flash continuously, until the FLASHW_STOP flag is set.        
        private const UInt32 FLASHW_TIMERNOFG = 12; //Flash continuously until the window comes to the foreground.  


        [StructLayout(LayoutKind.Sequential)]
        private struct FLASHWINFO
        {
            public UInt32 cbSize; //The size of the structure in bytes.            
            public IntPtr hwnd; //A Handle to the Window to be Flashed. The window can be either opened or minimized.


            public UInt32 dwFlags; //The Flash Status.            
            public UInt32 uCount; // number of times to flash the window            
            public UInt32 dwTimeout; //The rate at which the Window is to be flashed, in milliseconds. If Zero, the function uses the default cursor blink rate.        
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        public static void FlashWindow()
        {
            FLASHWINFO info = new FLASHWINFO
            {
                hwnd = Process.GetCurrentProcess().MainWindowHandle,
                dwFlags = FLASHW_ALL | FLASHW_TIMER,
                uCount = 1,
                dwTimeout = 0
            };

            info.cbSize = Convert.ToUInt32(Marshal.SizeOf(info));
            FlashWindowEx(ref info);
        }

        public static void StopFlashingWindow()
        {
            FLASHWINFO info = new FLASHWINFO();
            info.hwnd = Process.GetCurrentProcess().MainWindowHandle;
            info.cbSize = Convert.ToUInt32(Marshal.SizeOf(info));
            info.dwFlags = FLASHW_STOP;
            info.uCount = UInt32.MaxValue;
            info.dwTimeout = 0;
            FlashWindowEx(ref info);
        }
        class Locs
        {
            public readonly static IntPtr shardName = (IntPtr)0x0098DE48;

            public readonly static IntPtr windowWidth = (IntPtr)0x0098DFD0;
            public readonly static IntPtr windowHeight = (IntPtr)0x0098DFD4;

            public readonly static IntPtr charName = (IntPtr)0x0098E008;

            public readonly static IntPtr currHp = (IntPtr)0x0098E02E;
            public readonly static IntPtr maxHp = (IntPtr)0x0098E030;
            public readonly static IntPtr currStam = (IntPtr)0x0098E032;
            public readonly static IntPtr maxStam = (IntPtr)0x0098E034;
            public readonly static IntPtr currMp = (IntPtr)0x0098E036;
            public readonly static IntPtr maxMp = (IntPtr)0x0098E038;

            public readonly static IntPtr currGold = (IntPtr)0x0098E03C;
            public readonly static IntPtr currWeight = (IntPtr)0x0098E040;
            public readonly static IntPtr MaxWeight = (IntPtr)0x0098E042;

            public readonly static IntPtr currPhysRes = (IntPtr)0x0098E044;



            public readonly static IntPtr currFireRes = (IntPtr)0x0098E04C;
            public readonly static IntPtr currColdRes = (IntPtr)0x0098E04E;
            public readonly static IntPtr currPoisRes = (IntPtr)0x0098E050;
            public readonly static IntPtr currEneRes = (IntPtr)0x0098E052;

            public readonly static IntPtr maxFireRes = (IntPtr)0x0098E054;
            public readonly static IntPtr maxColdRes = (IntPtr)0x0098E056;
            public readonly static IntPtr maxPoisRes = (IntPtr)0x0098E058;
            public readonly static IntPtr maxEneRes = (IntPtr)0x0098E5A;

            public readonly static IntPtr lowerManaCost = (IntPtr)0x0098E64;
            public readonly static IntPtr lowerRegCost = (IntPtr)0x0098E68;

            public readonly static IntPtr fastCastRecovery = (IntPtr)0x0098E6C;
            public readonly static IntPtr fastCast = (IntPtr)0x0098E6E;

            public readonly static IntPtr luck = (IntPtr)0x0098E70;

            public readonly static IntPtr maxDamage = (IntPtr)0x0098E72;
            public readonly static IntPtr minDamage = (IntPtr)0x0098E74;

            public readonly static IntPtr LastObjectID = (IntPtr)0x00AB33F0;
            public readonly static IntPtr LastTargetID = (IntPtr)0x00AB33FC;
            public readonly static IntPtr LastTargetType = (IntPtr)0x00AB3404;

            public readonly static IntPtr yPos = (IntPtr)0x00AB3A10;
            public readonly static IntPtr xPos = (IntPtr)0x00AB3A14;
            public readonly static IntPtr facet = (IntPtr)0x00AB3A18;

            public readonly static IntPtr LastSpellID = (IntPtr)0x00AB5B64;

            public readonly static IntPtr TargetingCursor = (IntPtr)0x00AB8A24;
            public readonly static IntPtr LastDraggedObjectID = (IntPtr)0x00AF9730;


            public readonly static IntPtr SkillToggles = (IntPtr)0x01F6BAC4;
            public readonly static IntPtr SkillCaps = (IntPtr)0x01F6BB00;
            public readonly static IntPtr SkillReal = (IntPtr)0x01F6BB78;
            public readonly static IntPtr SkillDisplayed = (IntPtr)0x01F6BBF0;


            public readonly static IntPtr charFacing = (IntPtr)0x1209D9E3;
            

        };

        class GameVals
        {
            public static string shardName => RAsciiStr(Locs.shardName);
            public static string charName => RAsciiStr(Locs.charName);

            public static int xPos => RInt32(Locs.xPos);
            public static int yPos => RInt32(Locs.yPos);
            public static string Facet {
                get {
                    string name;
                    switch (RInt8(Locs.facet))
                    {
                        case 0:
                            name = "Felucca";
                            break;
                        case 1:
                            name = "Trammel";
                            break;
                        case 2:
                            name = "Ilshenar";
                            break;
                        case 3:
                            name = "Malas";
                            break;
                        case 4:
                            name = "Tokuno";
                            break;
                        case 5:
                            name = "Ter Mur";
                            break;
                        default:
                            name = "Unknown";
                            break;
                    }
                    return name;
                }
            }


            
        }
        const int PROCESS_ALL_ACCESS = 0x1f0fff;
        private static IntPtr UOHandle = IntPtr.Zero;

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        static internal extern bool CloseHandle(IntPtr hObject);
        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        static internal extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, int dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, int dwCreationFlags, IntPtr lpThreadId);
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        static internal extern IntPtr OpenProcess(int dwDesiredAcess, bool bInheritHandle, int dwProcessId);
        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int iSize, IntPtr lpNumberOfBytesRead);
        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int iSize, IntPtr lpNumberOfBytesWritten);

        public bool TryAttachToClient()
        {
            Process selectedProcess = null;
            Process[] _allProcesses = Process.GetProcesses();
            try
            {
                var potentialProcesses = new List<Process>();
                foreach (Process proc in _allProcesses)
                {
                    if (proc.MainWindowTitle.ToUpper().IndexOf("ULTIMA ONLINE") == 0)
                    {
                        potentialProcesses.Add(proc);
                    }
                }

                if (potentialProcesses.Count == 0)
                {
                    return false;
                }
                else if (potentialProcesses.Count >= 1)
                {
                    selectedProcess = potentialProcesses[0];
                }

                if (selectedProcess != null)
                {
                    UOHandle = OpenProcess(PROCESS_ALL_ACCESS, false, selectedProcess.Id);
                    return true;
                }
            }
            catch { };
            return false;
        }

        public static sbyte RInt8(IntPtr addr)
        {
            byte[] ByteBuffer = new byte[1];
            ReadProcessMemory(UOHandle, addr, ByteBuffer, 1, IntPtr.Zero);
            return (sbyte)ByteBuffer[0];
        }
        public static short RInt16(IntPtr addr)
        {
            byte[] ByteBuffer = new byte[2];
            ReadProcessMemory(UOHandle, addr, ByteBuffer, 2, IntPtr.Zero);
            return BitConverter.ToInt16(ByteBuffer, 0);
        }
        public static int RInt32(IntPtr addr)
        {
            byte[] ByteBuffer = new byte[4];
            ReadProcessMemory(UOHandle, addr, ByteBuffer, 4, IntPtr.Zero);
            return BitConverter.ToInt32(ByteBuffer, 0);
        }
        public static long RInt64(IntPtr addr)
        {
            byte[] ByteBuffer = new byte[8];
            ReadProcessMemory(UOHandle, addr, ByteBuffer, 8, IntPtr.Zero);
            return BitConverter.ToInt64(ByteBuffer, 0);
        }
        public static ushort RUInt16(IntPtr addr)
        {
            byte[] ByteBuffer = new byte[2];
            ReadProcessMemory(UOHandle, addr, ByteBuffer, 2, IntPtr.Zero);
            return BitConverter.ToUInt16(ByteBuffer, 0);
        }
        public static uint RUInt32(IntPtr addr)
        {
            byte[] ByteBuffer = new byte[4];
            ReadProcessMemory(UOHandle, addr, ByteBuffer, 4, IntPtr.Zero);
            return BitConverter.ToUInt32(ByteBuffer, 0);
        }
        public static ulong RUInt64(IntPtr addr)
        {
            byte[] ByteBuffer = new byte[8];
            ReadProcessMemory(UOHandle, addr, ByteBuffer, 8, IntPtr.Zero);
            return BitConverter.ToUInt64(ByteBuffer, 0);
        }
        public static float RFloat(IntPtr addr)
        {
            byte[] ByteBuffer = new byte[4];
            ReadProcessMemory(UOHandle, addr, ByteBuffer, 4, IntPtr.Zero);
            return BitConverter.ToSingle(ByteBuffer, 0);
        }
        public static double RDouble(IntPtr addr)
        {
            byte[] ByteBuffer = new byte[8];
            ReadProcessMemory(UOHandle, addr, ByteBuffer, 8, IntPtr.Zero);
            return BitConverter.ToDouble(ByteBuffer, 0);
        }
        public static IntPtr RIntPtr(IntPtr addr)
        {
            if (IntPtr.Size == 4)
            {
                byte[] ByteBuffer = new byte[4];
                ReadProcessMemory(UOHandle, addr, ByteBuffer, IntPtr.Size, IntPtr.Zero);
                return new IntPtr(BitConverter.ToInt32(ByteBuffer, 0));
            }
            else
            {
                byte[] ByteBuffer = new byte[8];
                ReadProcessMemory(UOHandle, addr, ByteBuffer, IntPtr.Size, IntPtr.Zero);
                return new IntPtr(BitConverter.ToInt64(ByteBuffer, 0));
            }
        }
        public static byte[] RBytes(IntPtr addr, int size)
        {
            byte[] _rtnBytes = new byte[size];
            ReadProcessMemory(UOHandle, addr, _rtnBytes, size, IntPtr.Zero);
            return _rtnBytes;
        }
        public static byte RByte(IntPtr addr)
        {
            byte[] ByteBuffer = new byte[1];
            ReadProcessMemory(UOHandle, addr, ByteBuffer, 1, IntPtr.Zero);
            return ByteBuffer[0];
        }
        public static string RAsciiStr(IntPtr addr, int maxLength = 0x100)
        {
            System.Text.StringBuilder Str = new System.Text.StringBuilder(maxLength);
            int loc = 0;

            var nextChr = '?';


            if (maxLength != 0)
            {
                byte[] bytes = new byte[2];

                while ((maxLength < 0 || loc < maxLength))
                {
                    ReadProcessMemory(UOHandle, IntPtr.Add(addr, loc), bytes, 1, IntPtr.Zero);
                    nextChr = System.Text.Encoding.ASCII.GetChars(bytes)[0];

                    if (nextChr == (char)0)
                    {
                        break; // TODO: might not be correct. Was : Exit While
                    }
                    else
                    {
                        Str.Append(nextChr);
                    }

                    loc += 1;
                }

            }

            return Str.ToString();
        }
        public static string RUnicodeStr(IntPtr addr, int maxLength = 0x100)
        {
            System.Text.StringBuilder Str = new System.Text.StringBuilder(maxLength);
            int loc = 0;

            var nextChr = '?';


            if (maxLength != 0)
            {
                byte[] bytes = new byte[3];
                
                while ((maxLength < 0 || loc < maxLength))
                {
                    ReadProcessMemory(UOHandle, IntPtr.Add(addr, loc * 2), bytes, 2, IntPtr.Zero);
                    nextChr = System.Text.Encoding.Unicode.GetChars(bytes)[0];

                    if (nextChr == (char)0)
                    {
                        break; // TODO: might not be correct. Was : Exit While
                    }
                    else
                    {
                        Str.Append(nextChr);
                    }

                    loc += 1;
                }

            }

            return Str.ToString();
        }

        private void startSkillTimer()
        {
            Timer skillTime = new System.Timers.Timer();
            skillTime.Interval = 1000;
            skillTime.Elapsed += new System.Timers.ElapsedEventHandler(skillTimeElapsed);
            skillTime.Enabled = true;
        }
        private void startCharTimer()
        {
            Timer charTime = new System.Timers.Timer();
            charTime.Interval = 333;
            charTime.Elapsed += new System.Timers.ElapsedEventHandler(charTimeElapsed);
            charTime.Enabled = true;
        }

        private string SlashSplit(string val, int index)
        {
            try { return val.Split('/')[index]; }
            catch { }
            return val;
        }

        private void charTimeElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    lore = new Gump(RIntPtr(RIntPtr((IntPtr)0x98EAA0) + 0x58));

                    if (lore.GumpName == "generic gump" && lore.pageNum == parsedPage + 1)
                    {
                        System.Threading.Thread.Sleep(333);
                        Rectangle bounds = new Rectangle();
                        bounds.Width = Convert.ToInt32(310);
                        bounds.Height = Convert.ToInt32(349);
                        bitmap = new Bitmap(bounds.Width, bounds.Height);

                        int pageNum = lore.pageNum;

                        using (Graphics g = Graphics.FromImage(bitmap))
                        {
                            g.CopyFromScreen(
                                new Point(lore.xPos,
                                    lore.yPos + Convert.ToInt32(SystemParameters.WindowCaptionHeight)),
                                Point.Empty,
                                bounds.Size
                                );
                        }

                        int x;

                        switch (pageNum)
                        {
                            case 1:
                                animal.objId = txtLastTargetID.Text;
                                pixCmp = Color.FromArgb(0, 0, 0, 132);
                                animal.Name = parseLine(40, 44);


                                x = 160;
                                pixCmp = Color.FromArgb(0, 0, 0, 10);
                                animal.HP = parseLine(160, 104);
                                animal.HP = SlashSplit(animal.HP, 1);
                                animal.Stam = parseLine(160, 122);
                                animal.Stam = SlashSplit(animal.Stam, 1);
                                animal.Mana = parseLine(160, 140);
                                animal.Mana = SlashSplit(animal.Mana, 1);
                                animal.Str = parseLine(160, 158);
                                animal.Dex = parseLine(160, 176);
                                animal.Int = parseLine(160, 194);
                                animal.BardDiff = parseLine(160, 212);

                                parsedPage++;
                                break;

                            case 2:
                                x = 190;
                                pixCmp = Color.FromArgb(0, 0, 0, 10);
                                animal.HPR = parseLine(190, 104);
                                animal.SR = parseLine(190, 122);
                                animal.MR = parseLine(190, 140);

                                parsedPage++;
                                break;
                            case 3:
                                x = 190;
                                pixCmp = Color.FromArgb(0, 0, 0, 10);
                                animal.PhysRes = parseLine(190, 104).Trim('%');

                                pixCmp = Color.FromArgb(0, 255, 0, 0);
                                animal.FireRes = parseLine(190, 122).Trim('%');

                                pixCmp = Color.FromArgb(0, 0, 0, 132);
                                animal.ColdRes = parseLine(190, 140).Trim('%');

                                pixCmp = Color.FromArgb(0, 0, 132, 0);
                                animal.PoisRes = parseLine(190, 158).Trim('%');

                                pixCmp = Color.FromArgb(0, 189, 132, 255);
                                animal.EnerRes = parseLine(190, 176).Trim('%');

                                parsedPage++;
                                break;
                            case 4:
                                x = 190;
                                pixCmp = Color.FromArgb(0, 0, 0, 10);
                                animal.PhysDam = parseLine(190, 104).Trim('%');

                                pixCmp = Color.FromArgb(0, 255, 0, 0);
                                animal.FireDam = parseLine(190, 122).Trim('%');

                                pixCmp = Color.FromArgb(0, 0, 0, 132);
                                animal.ColdDam = parseLine(190, 140).Trim('%');

                                pixCmp = Color.FromArgb(0, 0, 132, 0);
                                animal.PoisDam = parseLine(190, 158).Trim('%');

                                pixCmp = Color.FromArgb(0, 189, 132, 255);
                                animal.EnerDam = parseLine(190, 176).Trim('%');

                                pixCmp = Color.FromArgb(0, 0, 0, 10);
                                animal.BaseDam = parseLine(190, 194).Trim('%');

                                parsedPage++;
                                break;
                            case 5:
                                x = 160;
                                pixCmp = Color.FromArgb(0, 0, 0, 10);
                                animal.WresCurr = parseLine(160, 104);
                                animal.WresCurr = SlashSplit(animal.WresCurr, 0);
                                animal.TactCurr = parseLine(160, 122);
                                animal.TactCurr = SlashSplit(animal.TactCurr, 0);
                                animal.ResistCurr = parseLine(160, 140);
                                animal.ResistCurr = SlashSplit(animal.ResistCurr, 0);
                                animal.AnatCurr = parseLine(160, 158);
                                animal.AnatCurr = SlashSplit(animal.AnatCurr, 0);
                                animal.HealCurr = parseLine(160, 176);
                                animal.HealCurr = SlashSplit(animal.HealCurr, 0);
                                animal.PoisCurr = parseLine(160, 194);
                                animal.PoisCurr = SlashSplit(animal.PoisCurr, 0);
                                animal.DetectCurr = parseLine(160, 212);
                                animal.DetectCurr = SlashSplit(animal.DetectCurr, 0);
                                animal.HideCurr = parseLine(160, 230);
                                animal.HideCurr = SlashSplit(animal.HideCurr, 0);
                                animal.ParryCurr = parseLine(160, 248);
                                animal.ParryCurr = SlashSplit(animal.ParryCurr, 0);

                                animal.WresCap = parseLine(160, 104);
                                animal.WresCap = SlashSplit(animal.WresCap, 1);
                                animal.TactCap = parseLine(160, 122);
                                animal.TactCap = SlashSplit(animal.TactCap, 1);
                                animal.ResistCap = parseLine(160, 140);
                                animal.ResistCap = SlashSplit(animal.ResistCap, 1);
                                animal.AnatCap = parseLine(160, 158);
                                animal.AnatCap = SlashSplit(animal.AnatCap, 1);
                                animal.HealCap = parseLine(160, 176);
                                animal.HealCap = SlashSplit(animal.HealCap, 1);
                                animal.PoisCap = parseLine(160, 194);
                                animal.PoisCap = SlashSplit(animal.PoisCap, 1);
                                animal.DetectCap = parseLine(160, 212);
                                animal.DetectCap = SlashSplit(animal.DetectCap, 1);
                                animal.HideCap = parseLine(160, 230);
                                animal.HideCap = SlashSplit(animal.HideCap, 1);
                                animal.ParryCap = parseLine(160, 248);
                                animal.ParryCap = SlashSplit(animal.ParryCap, 1);

                                parsedPage++;
                                break;
                            case 6:
                                x = 160;
                                pixCmp = Color.FromArgb(0, 0, 0, 10);
                                animal.MageCurr = parseLine(160, 104);
                                animal.MageCurr = SlashSplit(animal.MageCurr, 0);
                                animal.EvalCurr = parseLine(160, 122);
                                animal.EvalCurr = SlashSplit(animal.EvalCurr, 0);
                                animal.MedCurr = parseLine(160, 140);
                                animal.MedCurr = SlashSplit(animal.MedCurr, 0);
                                animal.NecroCurr = parseLine(160, 158);
                                animal.NecroCurr = SlashSplit(animal.NecroCurr, 0);
                                animal.SpiritCurr = parseLine(160, 176);
                                animal.SpiritCurr = SlashSplit(animal.SpiritCurr, 0);
                                animal.MystCurr = parseLine(160, 194);
                                animal.MystCurr = SlashSplit(animal.MystCurr, 0);
                                animal.FocusCurr = parseLine(160, 212);
                                animal.FocusCurr = SlashSplit(animal.FocusCurr, 0);
                                animal.SWCurr = parseLine(160, 230);
                                animal.SWCurr = SlashSplit(animal.SWCurr, 0);
                                animal.DiscoCurr = parseLine(160, 248);
                                animal.DiscoCurr = SlashSplit(animal.DiscoCurr, 0);

                                animal.MageCap = parseLine(160, 104);
                                animal.MageCap = SlashSplit(animal.MageCap, 1);
                                animal.EvalCap = parseLine(160, 122);
                                animal.EvalCap = SlashSplit(animal.EvalCap, 1);
                                animal.MedCap = parseLine(160, 140);
                                animal.MedCap = SlashSplit(animal.MedCap, 1);
                                animal.NecroCap = parseLine(160, 158);
                                animal.NecroCap = SlashSplit(animal.NecroCap, 1);
                                animal.SpiritCap = parseLine(160, 176);
                                animal.SpiritCap = SlashSplit(animal.SpiritCap, 1);
                                animal.MystCap = parseLine(160, 194);
                                animal.MystCap = SlashSplit(animal.MystCap, 1);
                                animal.FocusCap = parseLine(160, 212);
                                animal.FocusCap = SlashSplit(animal.FocusCap, 1);
                                animal.SWCap = parseLine(160, 230);
                                animal.SWCap = SlashSplit(animal.SWCap, 1);
                                animal.DiscoCap = parseLine(160, 248);
                                animal.DiscoCap = SlashSplit(animal.DiscoCap, 1);

                                parsedPage++;
                                break;
                            case 7:
                                x = 160;
                                pixCmp = Color.FromArgb(0, 0, 0, 10);
                                animal.BushCurr = parseLine(160, 104);
                                animal.BushCurr = SlashSplit(animal.BushCurr, 0);
                                animal.NinjCurr = parseLine(160, 122);
                                animal.NinjCurr = SlashSplit(animal.NinjCurr, 0);
                                animal.ChivCurr = parseLine(160, 140);
                                animal.ChivCurr = SlashSplit(animal.ChivCurr, 0);

                                animal.BushCap = parseLine(160, 104);
                                animal.BushCap = SlashSplit(animal.BushCap, 1);
                                animal.NinjCap = parseLine(160, 122);
                                animal.NinjCap = SlashSplit(animal.NinjCap, 1);
                                animal.ChivCap = parseLine(160, 140);
                                animal.ChivCap = SlashSplit(animal.ChivCap, 1);

                                x = 30;
                                pixCmp = Color.FromArgb(0, 82, 66, 41);
                                try
                                {
                                    animal.Slots = parseLine(30, 248)[0].ToString();
                                }
                                catch { animal.Slots = "?"; }

                                parsedPage++;
                                break;
                            case 8:


                                btnCapture.IsEnabled = true;
                                btnCaptureStop.IsEnabled = false;
                                parsedPage++;
                                break;
                        }

                        //bitmap.Save("c:\\temp\\test.bmp", ImageFormat.Bmp);
                    }


                    //this.Title = $@"dUO - {RInt16(Locs.currWeight)}/{RInt16(Locs.MaxWeight)}";
                    txtLastTargetID.Text = RInt32(Locs.LastTargetID).ToString("X");
                    txtLastTargetType.Text = RInt32(Locs.LastTargetType).ToString("X");
                    txtLastObjectID.Text = RInt32(Locs.LastObjectID).ToString("X");

                    foreach (PropVal pv in dgLore.Items)
                    {
                        pv.Val = animal[pv.Prop];
                    }

                    dgLore.Items.Refresh();
                       


                });
            }
            catch
            { };
        }


        private void skillTimeElapsed(object sender, ElapsedEventArgs e)
        {
            foreach (Skill skl in dgSkills.Items)
            {
                Int16 prevSkill = skl.SkillVal;
                DateTime prevGain = skl.GainTime;

                string[] states = { "Up", "Down", "Locked" };

                skl.GainState = states[RInt8(Locs.SkillToggles + skl.SkillNum)];
                skl.SkillCap = RInt16(Locs.SkillCaps + skl.SkillNum * 2);
                skl.SkillVal = RInt16(Locs.SkillReal + skl.SkillNum * 2);
                skl.SinceLast = DateTime.Now - skl.GainTime;
                skl.TimeSinceGain = skl.SinceLast.ToString(@"hh\:mm\:ss");

                int minutesPerGain;
                switch (skl.SkillVal)
                {
                    case Int16 n when (n >= 700 && n < 800):
                        minutesPerGain = 5;
                        break;
                    case Int16 n when (n >= 800 && n < 900):
                        minutesPerGain = 8;
                        break;
                    case Int16 n when (n >= 900 && n < 1000):
                        minutesPerGain = 12;
                        break;
                    case Int16 n when (n >= 1000 && n < 1100):
                        minutesPerGain = 15;
                        break;
                    case Int16 n when (n >= 1100 && n < 1200):
                        minutesPerGain = 15;
                        break;
                    default:
                        minutesPerGain = 0;
                        break;
                }
                
                if ((prevSkill > 0) && (skl.SkillVal - prevSkill) > 0)
                    skl.GainTime = DateTime.Now;

                if (skl.GainState != "Up")
                    skl.TimeSinceGain = "";


                if ((minutesPerGain > 0) && (minutesPerGain <= skl.SinceLast.Minutes) && (skl.GainState == "Up"))
                {
                    FlashWindow();
                }
            }
            this.Dispatcher.Invoke(() =>
            {
                dgSkills.Items.Refresh();
            });
        }

        public class Gump
        {
            public IntPtr gumpLoc { get; set; }
            public string GumpName => RAsciiStr(RIntPtr(gumpLoc + 0x8));
            public Int32 xPos => RInt32(gumpLoc + 0x34);
            public Int32 yPos => RInt32(gumpLoc + 0x38);
            public Int32 pageNum => RInt32(gumpLoc + 0xBC);

            public Gump()
            {
                gumpLoc = (IntPtr)0;
            }
            public Gump(IntPtr gumpLoc)
            {
                this.gumpLoc = gumpLoc;
            }
        }

        public class Animal
        {
            public string this[string propertyName]
            {
                get
                {
                    PropertyInfo property = GetType().GetProperty(propertyName);
                    return property.GetValue(this, null).ToString();
                }
                set
                {
                    PropertyInfo property = GetType().GetProperty(propertyName);
                    property.SetValue(this, value, null);
                }
            }

            public string Name { get; set; }
            public string objId { get; set; }
            public string HP { get; set; }
            public string Stam { get; set; }
            public string Mana { get; set; }
            public string Str { get; set; }
            public string Dex { get; set; }
            public string Int { get; set; }
            public string BardDiff { get; set; }

            public string HPR { get; set; }
            public string SR { get; set; }
            public string MR { get; set; }

            public string PhysRes { get; set; }
            public string FireRes { get; set; }
            public string ColdRes { get; set; }
            public string PoisRes { get; set; }
            public string EnerRes { get; set; }

            public string PhysDam { get; set; }
            public string FireDam { get; set; }
            public string ColdDam { get; set; }
            public string PoisDam { get; set; }
            public string EnerDam { get; set; }
            public string BaseDam { get; set; }

            public string WresCurr { get; set; }
            public string TactCurr { get; set; }
            public string ResistCurr { get; set; }
            public string AnatCurr { get; set; }
            public string HealCurr { get; set; }
            public string PoisCurr { get; set; }
            public string DetectCurr { get; set; }
            public string HideCurr { get; set; }
            public string ParryCurr { get; set; }

            public string MageCurr { get; set; }
            public string EvalCurr { get; set; }
            public string MedCurr { get; set; }
            public string NecroCurr { get; set; }
            public string SpiritCurr { get; set; }
            public string MystCurr { get; set; }
            public string FocusCurr { get; set; }
            public string SWCurr { get; set; }
            public string DiscoCurr { get; set; }

            public string BushCurr { get; set; }
            public string NinjCurr { get; set; }
            public string ChivCurr { get; set; }
            public string WresCap { get; set; }
            public string TactCap { get; set; }
            public string ResistCap { get; set; }
            public string AnatCap { get; set; }
            public string HealCap { get; set; }
            public string PoisCap { get; set; }
            public string DetectCap { get; set; }
            public string HideCap { get; set; }
            public string ParryCap { get; set; }

            public string MageCap { get; set; }
            public string EvalCap { get; set; }
            public string MedCap { get; set; }
            public string NecroCap { get; set; }
            public string SpiritCap { get; set; }
            public string MystCap { get; set; }
            public string FocusCap { get; set; }
            public string SWCap { get; set; }
            public string DiscoCap { get; set; }

            public string BushCap { get; set; }
            public string NinjCap { get; set; }
            public string ChivCap { get; set; }
            public string Slots { get; set; }


            public Animal()
            {
                Name = "?";
                objId = "?";
                HP = "?";
                Stam = "?";
                Mana = "?";
                Str = "?";
                Dex = "?";
                Int = "?";
                BardDiff = "?";

                HPR = "?";
                SR = "?";
                MR = "?";

                PhysRes = "?";
                FireRes = "?";
                ColdRes = "?";
                PoisRes = "?";
                EnerRes = "?";

                PhysDam = "?";
                FireDam = "?";
                ColdDam = "?";
                PoisDam = "?";
                EnerDam = "?";
                BaseDam = "?";

                WresCurr = "?";
                WresCap = "?";
                TactCurr = "?";
                TactCap = "?";
                ResistCurr = "?";
                ResistCap = "?";
                AnatCurr = "?";
                AnatCap = "?";
                HealCurr = "?";
                HealCap = "?";
                PoisCurr = "?";
                PoisCap = "?";
                DetectCurr = "?";
                DetectCap = "?";
                HideCurr = "?";
                HideCap = "?";
                ParryCurr = "?";
                ParryCap = "?";

                MageCurr = "?";
                MageCap = "?";
                EvalCurr = "?";
                EvalCap = "?";
                MedCurr = "?";
                MedCap = "?";
                NecroCurr = "?";
                NecroCap = "?";
                SpiritCurr = "?";
                SpiritCap = "?";
                MystCurr = "?";
                MystCap = "?";
                FocusCurr = "?";
                FocusCap = "?";
                SWCurr = "?";
                SWCap = "?";
                DiscoCurr = "?";
                DiscoCap = "?";

                BushCurr = "?";
                BushCap = "?";
                NinjCurr = "?";
                NinjCap = "?";
                ChivCurr = "?";
                ChivCap = "?";
                Slots = "?";


            }



        }
        public class PropVal
        {
            public string Prop { get; set; }
            public string Val { get; set; }
            public PropVal(string prop, string val)
            {
                this.Prop = prop;
                this.Val = val;
            }
        }

        public class Skill
        {
            public Int16 SkillNum { get; set; }
            public string SkillName { get; set; }
            public Int16 SkillVal { get; set; }
            public Int16 SkillCap { get; set; }
            public string GainState { get; set; }
            public DateTime GainTime { get; set; }
            public TimeSpan SinceLast { get; set; }
            public string TimeSinceGain { get; set; }

            public Skill(Int16 SkillNum, string SkillName)
            {
                this.SkillNum = SkillNum;
                this.SkillName = SkillName;
                SkillVal = 0;
                SkillCap = 1000;
                GainState = "Unknown";
                GainTime = DateTime.Now;
                SinceLast = TimeSpan.Zero;
                TimeSinceGain = "0:00";
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            dgSkills.Items.Add(new Skill(01, "01"));
            dgSkills.Items.Add(new Skill(02, "Animal Lore"));
            dgSkills.Items.Add(new Skill(03, "Item Identification"));
            dgSkills.Items.Add(new Skill(04, "Arms Lore"));
            dgSkills.Items.Add(new Skill(05, "05"));
            dgSkills.Items.Add(new Skill(06, "06"));
            dgSkills.Items.Add(new Skill(07, "07"));
            dgSkills.Items.Add(new Skill(08, "Bowcraft/Fletching"));
            dgSkills.Items.Add(new Skill(09, "Peacemaking"));
            dgSkills.Items.Add(new Skill(10, "Camping"));
            dgSkills.Items.Add(new Skill(11, "Carpentry"));
            dgSkills.Items.Add(new Skill(12, "Cartography"));
            dgSkills.Items.Add(new Skill(13, "Cooking"));
            dgSkills.Items.Add(new Skill(14, "14"));
            dgSkills.Items.Add(new Skill(15, "Discordance"));
            dgSkills.Items.Add(new Skill(16, "16"));
            dgSkills.Items.Add(new Skill(17, "17"));
            dgSkills.Items.Add(new Skill(18, "Fishing"));
            dgSkills.Items.Add(new Skill(19, "Forensic Evaluation"));
            dgSkills.Items.Add(new Skill(20, "20"));
            dgSkills.Items.Add(new Skill(21, "Hiding"));
            dgSkills.Items.Add(new Skill(22, "22"));
            dgSkills.Items.Add(new Skill(23, "Inscription"));
            dgSkills.Items.Add(new Skill(24, "Lockpicking"));
            dgSkills.Items.Add(new Skill(25, "Magery"));
            dgSkills.Items.Add(new Skill(26, "26"));
            dgSkills.Items.Add(new Skill(27, "27"));
            dgSkills.Items.Add(new Skill(28, "28"));
            dgSkills.Items.Add(new Skill(29, "Musicianship"));
            dgSkills.Items.Add(new Skill(30, "30"));
            dgSkills.Items.Add(new Skill(31, "Archery"));
            dgSkills.Items.Add(new Skill(32, "32"));
            dgSkills.Items.Add(new Skill(33, "33"));
            dgSkills.Items.Add(new Skill(34, "34"));
            dgSkills.Items.Add(new Skill(35, "Animal Taming"));
            dgSkills.Items.Add(new Skill(36, "Taste Identification"));
            dgSkills.Items.Add(new Skill(37, "Tinkering"));
            dgSkills.Items.Add(new Skill(38, "38"));
            dgSkills.Items.Add(new Skill(39, "39"));
            dgSkills.Items.Add(new Skill(40, "40"));
            dgSkills.Items.Add(new Skill(41, "41"));
            dgSkills.Items.Add(new Skill(42, "42"));
            dgSkills.Items.Add(new Skill(43, "Wrestling"));
            dgSkills.Items.Add(new Skill(44, "44"));
            dgSkills.Items.Add(new Skill(45, "Mining"));
            dgSkills.Items.Add(new Skill(46, "Meditation"));
            dgSkills.Items.Add(new Skill(47, "47"));
            dgSkills.Items.Add(new Skill(48, "Remove Trap"));

            dgLore.Items.Add(new PropVal("Name", "?"));
            dgLore.Items.Add(new PropVal("objId", "?"));

            dgLore.Items.Add(new PropVal("HP", "?"));
            dgLore.Items.Add(new PropVal("Stam", "?"));
            dgLore.Items.Add(new PropVal("Mana", "?"));
            dgLore.Items.Add(new PropVal("Str", "?"));
            dgLore.Items.Add(new PropVal("Dex", "?"));
            dgLore.Items.Add(new PropVal("Int", "?"));
            dgLore.Items.Add(new PropVal("BardDiff", "?"));

            dgLore.Items.Add(new PropVal("HPR", "?"));
            dgLore.Items.Add(new PropVal("SR", "?"));
            dgLore.Items.Add(new PropVal("MR", "?"));

            dgLore.Items.Add(new PropVal("PhysRes", "?"));
            dgLore.Items.Add(new PropVal("FireRes", "?"));
            dgLore.Items.Add(new PropVal("ColdRes", "?"));
            dgLore.Items.Add(new PropVal("PoisRes", "?"));
            dgLore.Items.Add(new PropVal("EnerRes", "?"));

            dgLore.Items.Add(new PropVal("PhysDam", "?"));
            dgLore.Items.Add(new PropVal("FireDam", "?"));
            dgLore.Items.Add(new PropVal("ColdDam", "?"));
            dgLore.Items.Add(new PropVal("PoisDam", "?"));
            dgLore.Items.Add(new PropVal("EnerDam", "?"));
            dgLore.Items.Add(new PropVal("BaseDam", "?"));

            dgLore.Items.Add(new PropVal("WresCurr", "?"));
            dgLore.Items.Add(new PropVal("WresCap", "?"));
            dgLore.Items.Add(new PropVal("TactCurr", "?"));
            dgLore.Items.Add(new PropVal("TactCap", "?"));
            dgLore.Items.Add(new PropVal("ResistCurr", "?"));
            dgLore.Items.Add(new PropVal("ResistCap", "?"));
            dgLore.Items.Add(new PropVal("AnatCurr", "?"));
            dgLore.Items.Add(new PropVal("AnatCap", "?"));
            dgLore.Items.Add(new PropVal("HealCurr", "?"));
            dgLore.Items.Add(new PropVal("HealCap", "?"));
            dgLore.Items.Add(new PropVal("PoisCurr", "?"));
            dgLore.Items.Add(new PropVal("PoisCap", "?"));
            dgLore.Items.Add(new PropVal("DetectCurr", "?"));
            dgLore.Items.Add(new PropVal("DetectCap", "?"));
            dgLore.Items.Add(new PropVal("HideCurr", "?"));
            dgLore.Items.Add(new PropVal("HideCap", "?"));
            dgLore.Items.Add(new PropVal("ParryCurr", "?"));
            dgLore.Items.Add(new PropVal("ParryCap", "?"));

            dgLore.Items.Add(new PropVal("MageCurr", "?"));
            dgLore.Items.Add(new PropVal("MageCap", "?"));
            dgLore.Items.Add(new PropVal("EvalCurr", "?"));
            dgLore.Items.Add(new PropVal("EvalCap", "?"));
            dgLore.Items.Add(new PropVal("MedCurr", "?"));
            dgLore.Items.Add(new PropVal("MedCap", "?"));
            dgLore.Items.Add(new PropVal("NecroCurr", "?"));
            dgLore.Items.Add(new PropVal("NecroCap", "?"));
            dgLore.Items.Add(new PropVal("SpiritCurr", "?"));
            dgLore.Items.Add(new PropVal("SpiritCap", "?"));
            dgLore.Items.Add(new PropVal("MystCurr", "?"));
            dgLore.Items.Add(new PropVal("MystCap", "?"));
            dgLore.Items.Add(new PropVal("FocusCurr", "?"));
            dgLore.Items.Add(new PropVal("FocusCap", "?"));
            dgLore.Items.Add(new PropVal("SWCurr", "?"));
            dgLore.Items.Add(new PropVal("SWCap", "?"));
            dgLore.Items.Add(new PropVal("DiscoCurr", "?"));
            dgLore.Items.Add(new PropVal("DiscoCap", "?"));

            dgLore.Items.Add(new PropVal("BushCurr", "?"));
            dgLore.Items.Add(new PropVal("BushCap", "?"));
            dgLore.Items.Add(new PropVal("NinjCurr", "?"));
            dgLore.Items.Add(new PropVal("NinjCap", "?"));
            dgLore.Items.Add(new PropVal("ChivCurr", "?"));
            dgLore.Items.Add(new PropVal("ChivCap", "?"));
            dgLore.Items.Add(new PropVal("Slots", "?"));

            cmbAnimalType.Items.Add("Anon (Earth)");
            cmbAnimalType.Items.Add("Ant Lion");
            cmbAnimalType.Items.Add("Bake Kitsune");
            cmbAnimalType.Items.Add("Bake Kitsune (Legacy)");
            cmbAnimalType.Items.Add("Bane Dragon");
            cmbAnimalType.Items.Add("Black Solen Warrior");
            cmbAnimalType.Items.Add("Black Solen Worker");
            cmbAnimalType.Items.Add("Blood Fox");
            cmbAnimalType.Items.Add("Boar");
            cmbAnimalType.Items.Add("Bull");
            cmbAnimalType.Items.Add("Cat");
            cmbAnimalType.Items.Add("Chicken");
            cmbAnimalType.Items.Add("Coconut Crab");
            cmbAnimalType.Items.Add("Cold Drake");
            cmbAnimalType.Items.Add("Cougar");
            cmbAnimalType.Items.Add("Cow");
            cmbAnimalType.Items.Add("Crimson Drake");
            cmbAnimalType.Items.Add("Crow");
            cmbAnimalType.Items.Add("Cu Sidhe");
            cmbAnimalType.Items.Add("Deathwatch Beetle");
            cmbAnimalType.Items.Add("Dimetrosaur");
            cmbAnimalType.Items.Add("Dire Wolf");
            cmbAnimalType.Items.Add("Dog");
            cmbAnimalType.Items.Add("Dragon");
            cmbAnimalType.Items.Add("Dragon (Legacy)");
            cmbAnimalType.Items.Add("Dragon Wolf");
            cmbAnimalType.Items.Add("Drake");
            cmbAnimalType.Items.Add("Dread Spider");
            cmbAnimalType.Items.Add("Dread Warhorse");
            cmbAnimalType.Items.Add("Eowmu");
            cmbAnimalType.Items.Add("Fire Beetle");
            cmbAnimalType.Items.Add("Fire Steed");
            cmbAnimalType.Items.Add("Frost Dragon");
            cmbAnimalType.Items.Add("Frost Drake");
            cmbAnimalType.Items.Add("Frost Mite");
            cmbAnimalType.Items.Add("Frost Spider");
            cmbAnimalType.Items.Add("Gallusaurus");
            cmbAnimalType.Items.Add("Gaman");
            cmbAnimalType.Items.Add("Giant Beetle");
            cmbAnimalType.Items.Add("Goat");
            cmbAnimalType.Items.Add("Great Hart");
            cmbAnimalType.Items.Add("Greater Dragon");
            cmbAnimalType.Items.Add("Grizzled Mare");
            cmbAnimalType.Items.Add("Grizzly Bear");
            cmbAnimalType.Items.Add("Hell Hound");
            cmbAnimalType.Items.Add("Hellcat");
            cmbAnimalType.Items.Add("High Plains Boura");
            cmbAnimalType.Items.Add("Hind");
            cmbAnimalType.Items.Add("Hiryu");
            cmbAnimalType.Items.Add("Hungry Coconut Crab");
            cmbAnimalType.Items.Add("Iron Beetle");
            cmbAnimalType.Items.Add("Ki-Rin");
            cmbAnimalType.Items.Add("Lasher");
            cmbAnimalType.Items.Add("Lava Lizard");
            cmbAnimalType.Items.Add("Lesser Hiryu");
            cmbAnimalType.Items.Add("Lion");
            cmbAnimalType.Items.Add("Llama");
            cmbAnimalType.Items.Add("Mongbat");
            cmbAnimalType.Items.Add("Najasaurus");
            cmbAnimalType.Items.Add("Nightmare");
            cmbAnimalType.Items.Add("Nightmare (Legacy)");
            cmbAnimalType.Items.Add("Ossein Ram");
            cmbAnimalType.Items.Add("Phoenix");
            cmbAnimalType.Items.Add("Pig");
            cmbAnimalType.Items.Add("Platinum Drake");
            cmbAnimalType.Items.Add("Polar Bear");
            cmbAnimalType.Items.Add("Rabbit");
            cmbAnimalType.Items.Add("Raptor");
            cmbAnimalType.Items.Add("Raven");
            cmbAnimalType.Items.Add("Reptalon");
            cmbAnimalType.Items.Add("Rune Beetle");
            cmbAnimalType.Items.Add("Sabre-Toothed Tiger");
            cmbAnimalType.Items.Add("Saurosaurus");
            cmbAnimalType.Items.Add("Serpentine Dragon");
            cmbAnimalType.Items.Add("Shadow Iron Elemental");
            cmbAnimalType.Items.Add("Shadow Wyrm");
            cmbAnimalType.Items.Add("Skeletal Cat");
            cmbAnimalType.Items.Add("Skree");
            cmbAnimalType.Items.Add("Stone Slith");
            cmbAnimalType.Items.Add("Stygian Drake");
            cmbAnimalType.Items.Add("Swamp Dragon");
            cmbAnimalType.Items.Add("Timber Wolf");
            cmbAnimalType.Items.Add("Triceratops");
            cmbAnimalType.Items.Add("Triton");
            cmbAnimalType.Items.Add("Tropical Bird");
            cmbAnimalType.Items.Add("Tsuki Wolf");
            cmbAnimalType.Items.Add("Turkey");
            cmbAnimalType.Items.Add("Unicorn");
            cmbAnimalType.Items.Add("Vollem");
            cmbAnimalType.Items.Add("Vollem (Legacy)");
            cmbAnimalType.Items.Add("Walrus");
            cmbAnimalType.Items.Add("White Wolf");
            cmbAnimalType.Items.Add("White Wyrm");
            cmbAnimalType.Items.Add("White Wyrm (Legacy)");
            cmbAnimalType.Items.Add("Wild Tiger");
            cmbAnimalType.Items.Add("Windrunner");


            if (TryAttachToClient())
            {
                //startSkillTimer();
                startCharTimer();

                txtVal.Text = $@"{GameVals.shardName} - {GameVals.charName} - {GameVals.Facet}: {GameVals.xPos}x{RInt32(Locs.yPos)}";
            }
            else
                txtVal.Text = "Can't connect";
        }



        Bitmap bitmap;
        Color pixCmp = new Color();
        Animal animal = new Animal();
        Gump lore = new Gump();
        int parsedPage = 0;


        private bool pixOn(int x, int y)
        {
            Color pixel = bitmap.GetPixel(x, y);
            if (pixel.R > pixCmp.R)
                return false;
            if (pixel.G > pixCmp.G)
                return false;
            if (pixel.B > pixCmp.B)
                return false;
            return true;
        }

        private char charRec(ref int x, int y)
        {
            //IsDot?
            if (
                pixOn(x + 1, y + 1) &&
                pixOn(x, y + 1) &&
                !pixOn(x, y - 1) &&
                !pixOn(x, y - 2) &&
                !pixOn(x, y - 5) &&
                !pixOn(x + 1, y - 1)
                )
            {
                x += 2;
                return '.';
            }

            //IsSlash?
            if (
                !pixOn(x + 1, y)
                )
            {
                x += 8;
                return '/';
            }

            //IsPercent?
            if (
                !pixOn(x + 1, y + 1) &&
                !pixOn(x - 1, y + 1)
                )
            {
                x += 8;
                return '%';
            }

            //IsZero?
            if (
                pixOn(x, y - 6) &&
                !pixOn(x, y - 8) &&
                pixOn(x + 1, y + 1) &&
                pixOn(x + 2, y + 1) &&
                pixOn(x + 3, y + 1) &&
                !pixOn(x + 4, y - 4)
                )
            {
                x += 7;
                return '0';
            }

            //IsOne?
            if (
                pixOn(x + 1, y) &&
                pixOn(x - 1, y - 7) &&
                !pixOn(x - 2, y - 7)
                )
            {
                x += 2;
                return '1';
            }

            //IsTwo?
            if (
                pixOn(x + 1, y) &&
                pixOn(x, y + 1) &&
                pixOn(x + 2, y) &&
                pixOn(x + 2, y + 1)
                )
            {
                x += 7;
                return '2';
            }

            //IsThree?
            if (
                pixOn(x + 1, y) &&
                !pixOn(x, y + 1)  &&
                pixOn(x + 2, y + 1) &&
                pixOn(x + 4, y) &&
                !pixOn(x, y - 2) &&
                !pixOn(x, y - 5)
                )
            {
                x += 7;
                return '3';
            }

            //IsFour?
            if (
                pixOn(x + 1, y) &&
                pixOn(x, y - 1) &&
                pixOn(x, y + 1) &&
                pixOn(x + 2, y - 2) &&
                pixOn(x -1, y - 2)
                )
            {
                x += 2;
                return '4';
            }

            //IsFive?
            if (
                pixOn(x + 1, y) &&
                !pixOn(x, y - 2)&&
                !pixOn(x, y + 1)
                )
            {
                x += 7;
                return '5';
            }

            //IsSix?
            if (
                pixOn(x, y - 4) &&
                pixOn(x + 1, y) &&
                pixOn(x + 2, y) &&
                pixOn(x + 3, y - 5) &&
                !pixOn(x, y + 1)
                )
            {
                x += 7;
                return '6';
            }

            //IsSeven?
            if (
                pixOn(x + 1, y) &&
                pixOn(x - 1, y - 7) &&
                pixOn(x - 2, y - 7)
                )
            {
                x += 2;
                return '7';
            }

            //IsEight?
            if (
                !pixOn(x, y + 1) &&
                !pixOn(x + 3, y - 5) &&
                pixOn(x, y - 6) &&
                pixOn(x, y - 2) &&
                !pixOn(x, y - 3)
                )
            {
                x += 7;
                return '8';
            }

            //IsNine?
            if (
                pixOn(x - 1, y + 1) &&
                pixOn(x + 1, y + 1)
                )
            {
                x += 2;
                return '9';
            }

            //Is_A?
            if (
                pixOn(x, y - 6) &&
                !pixOn(x, y - 7) &&
                pixOn(x + 2, y - 3) &&
                pixOn(x + 6, y)
                )
            {
                x += 7;
                return 'A';
            }

            //Is_a?
            if (
                !pixOn(x, y + 1) &&
                !pixOn(x, y - 4) &&
                !pixOn(x + 3, y - 5) &&
                pixOn(x + 1, y - 1) &&
                !pixOn(x + 2, y + 1) &&
                !pixOn(x + 3, y + 2)
                )
            {
                x += 5;
                return 'a';
            }

            //Is_B?
            if (
                pixOn(x, y - 6) &&
                pixOn(x, y - 8) &&
                !pixOn(x + 6, y - 4) &&
                !pixOn(x + 1, y - 9) &&
                !pixOn(x + 6, y - 6) &&
                pixOn(x + 2, y - 4) &&
                pixOn(x + 6, y)
                )
            {
                x += 7;
                return 'B';
            }

            //Is_b?
            if (
                pixOn(x, y - 7) &&
                !pixOn(x, y - 8) &&
                pixOn(x + 2, y + 1) &&
                pixOn(x + 2, y - 4) &&
                !pixOn(x + 2, y - 7) &&
                !pixOn(x - 1, y - 6)

                )
            {
                x += 5;
                return 'b';
            }

            //Is_C?
            if (
                pixOn(x, y - 6) &&
                pixOn(x, y - 7) &&
                !pixOn(x, y - 8) &&
                pixOn(x + 2, y - 8) &&
                pixOn(x + 6, y) &&
                !pixOn(x + 6, y - 2)
                )
            {
                x += 7;
                return 'C';
            }

            //Is_c?
            if (
                !pixOn(x, y + 1) &&
                !pixOn(x + 2, y) &&
                pixOn(x + 3, y)
                )
            {
                x += 5;
                return 'c';
            }

            //Is_D?
            if (
                pixOn(x, y - 6) &&
                pixOn(x, y - 7) &&
                pixOn(x, y - 8) &&
                pixOn(x + 2, y - 8) &&
                pixOn(x + 6, y) &&
                !pixOn(x + 6, y + 1)
                )
            {
                x += 7;
                return 'D';
            }

            //Is_d?
            if (
                !pixOn(x, y + 1) &&
                !pixOn(x, y - 4) &&
                pixOn(x + 3, y - 5) &&
                pixOn(x + 1, y - 1) &&
                !pixOn(x + 2, y + 1) &&
                !pixOn(x + 3, y + 2)
                )
            {
                x += 5;
                return 'd';
            }

            //Is_E?
            if (
                pixOn(x, y - 7) &&
                pixOn(x, y - 8) &&
                pixOn(x + 2, y + 1) &&
                pixOn(x + 2, y - 4) &&
                pixOn(x + 2, y - 8) &&
                !pixOn(x + 2, y - 7) &&
                !pixOn(x - 1, y - 6)

                )
            {
                x += 6;
                return 'E';
            }

            //Is_e?
            if (
                !pixOn(x, y + 1) &&
                pixOn(x, y - 1) &&
                pixOn(x, y - 2) &&
                !pixOn(x + 2, y)
                )
            {
                x += 5;
                return 'e';
            }

            //Is_F?
            if (
                pixOn(x, y - 7) &&
                pixOn(x, y - 8) &&
                pixOn(x + 2, y - 4) &&
                pixOn(x + 2, y - 8) &&
                !pixOn(x + 2, y - 7) &&
                !pixOn(x - 1, y - 6) &&
                !pixOn(x + 2, y)

                )
            {
                x += 6;
                return 'F';
            }

            //Is_f?
            if (
                pixOn(x, y - 6) &&
                pixOn(x, y - 8) &&
                pixOn(x + 1, y - 9) &&
                pixOn(x + 2, y - 4)
                )
            {
                x += 4;
                return 'f';
            }

            //Is_G?
            if (
                pixOn(x, y - 6) &&
                !pixOn(x, y - 8) &&
                pixOn(x + 1, y + 1) &&
                pixOn(x + 2, y + 1) &&
                pixOn(x + 3, y + 1)
                )
            {
                x += 7;
                return 'G';
            }

            //Is_g?
            if (
                !pixOn(x, y + 1) &&
                pixOn(x + 1, y - 1) &&
                !pixOn(x + 2, y + 1) &&
                !pixOn(x, y - 4) &&
                pixOn(x + 3, y + 2)
                )
            {
                x += 5;
                return 'g';
            }

            //Is_H?
            if (
                pixOn(x, y - 6) &&
                pixOn(x, y - 8) &&
                pixOn(x + 6, y - 4) &&
                !pixOn(x + 1, y - 9) &&
                pixOn(x + 2, y - 4) &&
                pixOn(x + 6, y)
                )
            {
                x += 7;
                return 'H';
            }

            //Is_h?
            if (
                pixOn(x, y - 7) &&
                pixOn(x + 2, y - 4) &&
                !pixOn(x + 2, y - 7) &&
                !pixOn(x - 1, y - 6) &&
                !pixOn(x, y - 8)

                )
            {
                x += 5;
                return 'h';
            }

            //Is_I?
            if (
                pixOn(x, y - 8) &&
                !pixOn(x, y - 9) &&
                pixOn(x, y - 5) &&
                !pixOn(x + 2, y - 3) &&
                !pixOn(x + 2, y - 2) &&
                !pixOn(x + 2, y + 1) &&
                !pixOn(x + 2, y - 7)

                )
            {
                x += 2;
                Console.WriteLine("Big I");
                return 'I';
            }

            //Is_i?
            if (
                !pixOn(x, y - 5) &&
                pixOn(x, y - 7) &&
                !pixOn(x, y + 2)
                )
            {
                x += 2;
                return 'i';
            }

            //Is_J?
            if (
                !pixOn(x, y + 1) &&
                !pixOn(x + 3, y - 5) &&
                !pixOn(x, y - 6) &&
                pixOn(x, y - 2) &&
                !pixOn(x, y - 3)
                )
            {
                x += 7;
                return 'J';
            }

            //Is_j?
            if (
                !pixOn(x, y - 5) &&
                pixOn(x, y - 7) &&
                pixOn(x, y + 2) &&
                pixOn(x - 1, y + 3)
                )
            {
                x += 2;
                return 'j';
            }

            //Is_K?
            if (
                pixOn(x, y - 8) &&
                !pixOn(x, y - 9) &&
                pixOn(x, y - 5) &&
                pixOn(x + 2, y - 3) &&
                !pixOn(x + 2, y - 7)

                )
            {
                x += 7;
                return 'K';
            }

            //Is_k?
            if (
                pixOn(x, y - 7) &&
                !pixOn(x, y - 8) &&
                pixOn(x, y - 5) &&
                !pixOn(x + 2, y - 3) &&
                pixOn(x + 2, y - 2) &&
                !pixOn(x + 2, y - 7)

                )
            {
                x += 5;
                return 'k';
            }

            //Is_L?
            if (
                pixOn(x, y - 8) &&
                !pixOn(x, y - 9) &&
                pixOn(x, y - 5) &&
                !pixOn(x + 2, y - 3) &&
                !pixOn(x + 2, y - 2) &&
                pixOn(x + 2, y + 1) &&
                !pixOn(x + 2, y - 7)

                )
            {
                x += 6;
                Console.WriteLine("Big L");
                return 'L';
            }

            //Is_l?
            if (
                pixOn(x, y - 7) &&
                !pixOn(x + 2, y - 7) &&
                !pixOn(x - 1, y - 6) &&
                !pixOn(x, y - 8)

                )
            {
                x += 2;
                Console.WriteLine("Little l");
                return 'l';
            }

            //Is_M?
            if (
                pixOn(x, y - 7) &&
                !pixOn(x + 2, y - 7) &&
                !pixOn(x - 1, y - 6) &&
                !pixOn(x, y - 8)

                )
            {
                x += 8;
                return 'M';
            }

            //Is_m?
            if (
                !pixOn(x + 2, y) &&
                pixOn(x + 2, y - 4) &&
                pixOn(x + 3, y - 4) &&
                !pixOn(x + 4, y - 4) &&
                pixOn(x + 5, y - 4)
                )
            {
                x += 8;
                return 'm';
            }





            //Is_n?
            if (
                !pixOn(x + 2, y) &&
                pixOn(x, y - 1) &&
                pixOn(x + 2, y - 4)
                )
            {
                x += 5;
                return 'n';
            }

            //Is_o?
            if (
                !pixOn(x, y + 1) &&
                !pixOn(x - 1, y) &&
                pixOn(x + 1, y + 1) &&
                pixOn(x + 2, y + 1)
                )
            {
                x += 5;
                return 'o';
            }

            //Is_R
            if (
                pixOn(x, y + 1) &&
                pixOn(x, y - 8) &&
                pixOn(x + 6, y + 1)
                )
            {
                x += 7;
                return 'R';
            }

            //Is_r
            if (
                pixOn(x + 2, y - 3) &&
                !pixOn(x + 2, y - 4)
                )
            {
                x += 5;
                return 'r';
            }

            //Is_s?
            if (
                pixOn(x - 1, y + 1) &&
                pixOn(x - 2, y + 1) &&
                pixOn(x - 3, y + 1)
                )
            {
                x += 2;
                return 's';
            }

            //Is_w?
            if (
                !pixOn(x, y + 1) &&
                !pixOn(x - 1, y) &&
                !pixOn(x, y + 3) &&
                pixOn(x + 1, y + 1)
                )
            {
                x += 6;
                return 'w';
            }

            //Is_y?
            if (
                !pixOn(x, y + 1) &&
                pixOn(x + 1, y - 1) &&
                !pixOn(x + 2, y + 1) &&
                pixOn(x + 3, y + 2)
                )
            {
                x += 5;
                return 'y';
            }

            //Is_z?
            if (
                !pixOn(x, y - 1) &&
                pixOn(x + 1, y - 1)
                
                )
            {
                x += 5;
                return 'z';
            }

            return '_';
        }

        private string parseLine(int x, int y)
        {
            string tempStr = "";
            for (int i = x; i < 280; i++)
            {
                if (pixOn(i, y))
                {
                    char newChar = charRec(ref i, y);
                    tempStr += newChar;
                }
                else
                {
                    //Console.WriteLine($@"{i},{y} - {bitmap.GetPixel(i, y)}");
                    if (
                        !pixOn(i, y + 1) &&
                        !pixOn(i, y - 1) &&
                        pixOn(i, y - 3) &&
                        !pixOn(i, y - 5)
                        )
                    {
                        i += 5;
                        tempStr += '-';
                    }
                }
            }
            return tempStr;
        }

        

        private void btnCapture_Click(object sender, RoutedEventArgs e)
        {
            parsedPage = 0;
            btnCapture.IsEnabled = false;
            btnCaptureStop.IsEnabled = true;

            animal = new Animal();
        }

        private void btnCaptureStop_Click(object sender, RoutedEventArgs e)
        {
            btnCapture.IsEnabled = true;
            btnCaptureStop.IsEnabled = false;
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            string path = $@"C:\dUO";
            string filename = $@"{cmbAnimalType.Text}.csv";
            Directory.CreateDirectory(path);

            if (chkWild.IsChecked == true)
                filename = "Wild-" + filename;
            else
                filename = "Tame-" + filename;

            if (!File.Exists($@"{path}\{filename}"))
            {
                File.AppendAllText($@"{path}\{filename}", "Name,objId,HP,Stam,Mana,Str,Dex,Int,BardDiff,HPR,SR,MR,PhysRes,FireRes,ColdRes,PoisRes,EnerRes,PhysDam,FireDam,ColdDam,PoisDam,EnerDam,BaseDam,WresCurr,WresCap,TactCurr,TactCap,ResistCurr,ResistCap,AnatCurr,AnatCap,HealCurr,HealCap,PoisCurr,PoisCap,DetectCurr,DetectCap,HideCurr,HideCap,ParryCurr,ParryCap,MageCurr,MageCap,EvalCurr,EvalCap,MedCurr,MedCap,NecroCurr,NecroCap,SpiritCurr,SpiritCap,MystCurr,MystCap,FocusCurr,FocusCap,SWCurr,SWCap,DiscoCurr,DiscoCap,BushCurr,BushCap,NinjCurr,NinjCap,ChivCurr,ChivCap,Slots" + Environment.NewLine);
            }
            File.AppendAllText($@"{path}\{filename}", $@"{animal.Name},{animal.objId},{animal.HP},{animal.Stam},{animal.Mana},{animal.Str},{animal.Dex},{animal.Int},{animal.BardDiff},{animal.HPR},{animal.SR},{animal.MR},{animal.PhysRes},{animal.FireRes},{animal.ColdRes},{animal.PoisRes},{animal.EnerRes},{animal.PhysDam},{animal.FireDam},{animal.ColdDam},{animal.PoisDam},{animal.EnerDam},{animal.BaseDam},{animal.WresCurr},{animal.WresCap},{animal.TactCurr},{animal.TactCap},{animal.ResistCurr},{animal.ResistCap},{animal.AnatCurr},{animal.AnatCap},{animal.HealCurr},{animal.HealCap},{animal.PoisCurr},{animal.PoisCap},{animal.DetectCurr},{animal.DetectCap},{animal.HideCurr},{animal.HideCap},{animal.ParryCurr},{animal.ParryCap},{animal.MageCurr},{animal.MageCap},{animal.EvalCurr},{animal.EvalCap},{animal.MedCurr},{animal.MedCap},{animal.NecroCurr},{animal.NecroCap},{animal.SpiritCurr},{animal.SpiritCap},{animal.MystCurr},{animal.MystCap},{animal.FocusCurr},{animal.FocusCap},{animal.SWCurr},{animal.SWCap},{animal.DiscoCurr},{animal.DiscoCap},{animal.BushCurr},{animal.BushCap},{animal.NinjCurr},{animal.NinjCap},{animal.ChivCurr},{animal.ChivCap},{animal.Slots}" + Environment.NewLine);
        }

        private void btnCAH_Click(object sender, RoutedEventArgs e)
        {
            Animal tmpanml = animal;

            if (chkWild.IsChecked == true)
            {

                switch (cmbAnimalType.Text)
                {
                    case "Cu Sidhe":
                    case "Ossein Ram":
                        try
                        {
                            tmpanml.HP = (Math.Floor(float.Parse(tmpanml.HP) / 2f)).ToString();
                            tmpanml.Stam = (Math.Floor(float.Parse(tmpanml.Stam) / 2f)).ToString();
                           // tmpanml.Mana = (float.Parse(tmpanml.Mana) / 2f).ToString();
                            tmpanml.Str = (Math.Floor(float.Parse(tmpanml.Str) / 2f)).ToString();
                            tmpanml.Dex = (Math.Floor(float.Parse(tmpanml.Dex) / 2f)).ToString();
                            //tmpanml.Int = (float.Parse(tmpanml.Int) / 2f).ToString();
                        }
                        catch { }
                        
                        break;

                }

            }

            string url = $@"https://www.uo-cah.com/pet-intensity-calculator?creature={cmbAnimalType.Text}&hits={animal.HP}&stamina={animal.Stam}&mana={animal.Mana}&str={animal.Str}&dex={animal.Dex}&int={animal.Int}&rateminimum=1&physical={animal.PhysRes}&fire={animal.FireRes}&cold={animal.ColdRes}&poison={animal.PoisRes}&energy={animal.EnerRes}&target_physical=--&target_fire=--&target_cold=--&target_poison=--&target_energy=--&wrestling={animal.WresCap}&resistingspells={animal.ResistCap}&evalintel={animal.EvalCap}&tactics={animal.TactCap}&magery={animal.MageCap}&poisoning={animal.PoisCap}&mic=fresh#freshresults";


            Process.Start(url);
        }
    }
}
