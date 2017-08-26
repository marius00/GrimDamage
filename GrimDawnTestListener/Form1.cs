using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EvilsoftCommons;
using EvilsoftCommons.DllInjector;
using log4net;

namespace GrimDawnTestListener {
    public partial class Form1 : Form {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Form1));

        private Action<RegisterWindow.DataAndType> _registerWindowDelegate;
        private RegisterWindow _window;
        private InjectionHelper _injector;
        private ProgressChangedEventHandler _injectorCallbackDelegate;
        private bool isFirstMessage = true;
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            Logger.Debug("Starting..");
            _registerWindowDelegate = CustomWndProc;
            _window = new RegisterWindow("GDTestWindowClass", _registerWindowDelegate);

            _injectorCallbackDelegate = InjectorCallback;
            _injector = new InjectionHelper(new BackgroundWorker(), _injectorCallbackDelegate, false, "Grim Dawn", string.Empty, "ItemAssistantHook.dll");
            this.Closing += Form1_Closing;

        }

        private void Form1_Closing(object sender, CancelEventArgs e) {
            _injector?.Dispose();
            _injector = null;
        }

        class PointerHelper {
            public int Address; // may also be the value
            public bool Success;
            public int ErrorCode;
            public int Value;

            public bool IsPointerToPointer;
            public int ValueIfPtr2Ptr;

            private String toString(int val) {
                if (val == 0)
                    return string.Empty;

                var result = new string(new[] {
                    (char) ((val & 0xFF000000) >> 24),
                    (char) ((val & 0x00FF0000) >> 16),
                    (char) ((val & 0x0000FF00) >> 8),
                    (char) ((val & 0x000000FF))
                });
                if (!BitConverter.IsLittleEndian)
                    return result.Reverse().ToString();

                return result;
            }
            public string AddressIfString => toString(Address);
            public string ValueIfString => toString(Value);
            public string Ptr2PtrValueIfString => toString(ValueIfPtr2Ptr);
        }

        List<PointerHelper> Decode(byte[] data, int offset) {
            List<PointerHelper> result = new List<PointerHelper>();

            int numItems = data[offset++];
            for (int i = 0; i < numItems; i++) {
                int localOffset = 14*i + offset;
                PointerHelper item = new PointerHelper();

                item.Address = IOHelper.GetInt(data, localOffset);
                localOffset += 4;

                item.ErrorCode = IOHelper.GetInt(data, localOffset);
                localOffset += 4;

                item.Value = IOHelper.GetInt(data, localOffset);
                localOffset += 4;

                item.ValueIfPtr2Ptr = IOHelper.GetInt(data, localOffset);
                localOffset += 4;

                item.IsPointerToPointer = data[localOffset++] != 0;
                item.Success = data[localOffset++] != 0;

                String a = item.AddressIfString;
                String b = item.Ptr2PtrValueIfString;
                String c = item.ValueIfString;
                result.Add(item);
            }

            return result;
        }

        private void CustomWndProc(RegisterWindow.DataAndType bt) {
            // Most if not all actions may interact with SQL
            // SQL is done on the UI thread.
            if (InvokeRequired) {
                Invoke((MethodInvoker) delegate { CustomWndProc(bt); });
                return;
            }

            if (bt.Type == 1001/* CharacterApplyDamage */) {
                
                float f = IOHelper.GetFloat(bt.Data, 0);
                int PlayStatsDamageType = IOHelper.GetInt(bt.Data, 4);
                int CombatAttributeType = IOHelper.GetInt(bt.Data, 8);

                int ptr = IOHelper.GetInt(bt.Data, 12);

                //Logger.Debug($"Received CharacterApplyDamage({ptr}, {f}, {PlayStatsDamageType}, {CombatAttributeType})");
            }

            else if (bt.Type == 1002/* CharacterSubtractLife*/) {
                uint pos = 0;
                float f = IOHelper.GetFloat(bt.Data, pos);
                pos += 4;

                int PlayStatsDamageType = IOHelper.GetInt(bt.Data, (int)pos);
                pos += 4;

                bool a = bt.Data[pos++] != 0;
                bool b = bt.Data[pos++] != 0;

                int _this = IOHelper.GetInt(bt.Data, (int)pos);
                pos += 4;

                int exp = bt.Data.Length >= pos+4 ? IOHelper.GetInt(bt.Data, (int) pos) : -1;
                pos += 4;
                //Logger.Debug($"Received CharacterSubtractLife({f}, {PlayStatsDamageType}, {a}, {b}, {_this}, {exp})");
            }

            else if (bt.Type == 1003 /* Log event*/) {
                var ptrs = Decode(bt.Data, 0);

                String message = IOHelper.GetBytePrefixedString(bt.Data, 0);
                foreach (var ptr in ptrs) {
                    //Logger.Debug($"Log event: success: {ptr.Success}");
                }
                
            }
            else if (bt.Type == 45454) {
                string message = IOHelper.GetNullString(bt.Data, 0).Replace("\r", "").Replace("\n", "");
                Logger.Debug("Stringy: " + message);
                
            }
            else {
                Logger.Warn($"Got a message of type {bt.Type}");
            }

            if (isFirstMessage) {
                Logger.Debug("Window message received");
                isFirstMessage = false;
            }
        }

        private void InjectorCallback(object sender, ProgressChangedEventArgs e) {
            if (InvokeRequired) {
                Invoke((MethodInvoker)delegate { InjectorCallback(sender, e); });
            }
            else {
                //Logger.Debug("Injector callback");
            }
        }
    }
}
