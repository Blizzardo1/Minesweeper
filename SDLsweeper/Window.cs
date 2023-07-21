using SDL2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = SDL2.Point;

namespace SDLsweeper {
    internal abstract class Window : Renderer, IGameObject {
        #region Events
        public event EventHandler? AppDidEnterBackground;
        public event EventHandler? AppDidEnterForeground;
        public event EventHandler? AppLowMemory;
        public event EventHandler? AppTerminating;
        public event EventHandler? AppWillEnterBackground;
        public event EventHandler? AppWillEnterForeground;
        public event EventHandler< AudioDeviceEvent >? AudioDeviceAdded;
        public event EventHandler< AudioDeviceEvent >? AudioDeviceRemoved;
        public event EventHandler? ClipboardUpdate;
        public event EventHandler< ControllerAxisEvent >? ControllerAxisMotion;
        public event EventHandler< ControllerButtonEvent >? ControllerButtonDown;
        public event EventHandler< ControllerButtonEvent >? ControllerButtonUp;
        public event EventHandler< ControllerDeviceEvent >? ControllerDeviceAdded;
        public event EventHandler< ControllerDeviceEvent >? ControllerDeviceRemoved;
        public event EventHandler< ControllerDeviceEvent >? ControllerDeviceRemapped;
        public event EventHandler< DisplayEvent >? DisplayEvents;
        public event EventHandler< DollarGestureEvent >? DollarGesture;
        public event EventHandler< DollarGestureEvent >? DollarRecord;
        public event EventHandler< DropEvent >? DropBegin;
        public event EventHandler< DropEvent >? DropComplete;
        public event EventHandler< DropEvent >? DropFile;
        public event EventHandler< DropEvent >? DropText;
        public event EventHandler< TouchFingerEvent >? FingerDown;
        public event EventHandler< TouchFingerEvent >? FingerMotion;
        public event EventHandler< TouchFingerEvent >? FingerUp;
        public event EventHandler? FirstEvent;
        public event EventHandler? LastEvent;
        public event EventHandler< JoyAxisEvent >? JoyAxisMotion;
        public event EventHandler< JoyBallEvent >? JoyBallMotion;
        public event EventHandler< JoyButtonEvent >? JoyButtonDown;
        public event EventHandler< JoyButtonEvent >? JoyButtonUp;
        public event EventHandler< JoyDeviceEvent >? JoyDeviceAdded;
        public event EventHandler< JoyDeviceEvent >? JoyDeviceRemoved;
        public event EventHandler< JoyHatEvent >? JoyHatMotion;
        public event EventHandler< KeyboardEvent >? KeymapChanged;
        public event EventHandler< KeyboardEvent >? KeyDown;
        public event EventHandler< KeyboardEvent >? KeyUp;
        public event EventHandler< MouseButtonEvent >? MouseDown;
        public event EventHandler< MouseButtonEvent >? MouseUp;
        public event EventHandler< MouseMotionEvent >? MouseMove;
        public event EventHandler< MouseWheelEvent >? MouseWheel;
        public event EventHandler< MultiGestureEvent >? MultiGesture;
        public event EventHandler< QuitEvent >? Quit;
        public event EventHandler? RenderDeviceReset;
        public event EventHandler? RenderTargetsReset;
        public event EventHandler< SensorEvent >? SensorUpdate;
        public event EventHandler< SysWMEvent >? SysWMEvent;
        public event EventHandler< TextEditingEvent >? TextEditing;
        public event EventHandler< TextInputEvent >? TextInput;
        public event EventHandler< UserEvent >? UserEvent;
        public event EventHandler< WindowEvent >? WindowEvent;
        #endregion
        
        protected IntPtr WindowPtr { get; }
        
        protected Window(string title, Point location, Size size, WindowFlags flags) {
            X = location.X;
            Y = location.Y;
            Width = size.Width;
            Height = size.Height;

            WindowPtr = SDL.CreateWindow(title,
                X == 0x7FFFFFFF ? SDL.WINDOWPOS_CENTERED : X,
                Y == 0x7FFFFFFF ? SDL.WINDOWPOS_CENTERED : Y,
                Width,
                Height,
                flags);

            if (WindowPtr == IntPtr.Zero)
            {
                throw new Exception("Cannot create Window");
            }

            Initialize(SDL.CreateRenderer(WindowPtr, -1, RendererFlags.Accelerated));

            if (RendererPtr == IntPtr.Zero)
            {
                throw new Exception("Cannot create RendererPtr");
            }
        }

        /// <inheritdoc />
        public int X { get; protected set; }

        /// <inheritdoc />
        public int Y { get; protected set; }

        /// <inheritdoc />
        public int Width { get; protected set; }
        
        /// <inheritdoc />
        public int Height { get; protected set; }

        /// <inheritdoc />
        public abstract string Name { get; }

        /// <inheritdoc />
        public abstract void Draw();

        /// <inheritdoc />
        public virtual void Update()
        {
            HandleEvents();
        }

        protected void UpdatePosition(IntPtr window)
        {
            SDL.GetWindowPosition(window, out int x, out int y);
            X = x;
            Y = y;
        }

        protected void UpdateSize(IntPtr window)
        {
            SDL.GetWindowSize(window, out int w, out int h);
            Width = w;
            Height = h;
        }
        
        #region Event Handler

        private void HandleEvents() {
            while (SDL.PollEvent(out Event e) >= 1) {
                switch (e.Type) {
                    case EventType.FirstEvent:
                        OnFirstEvent(e);
                        break;
                    case EventType.Quit:
                        OnQuit(e.Quit);
                        break;
                    case EventType.AppTerminating:
                        OnAppTerminating(e);
                        break;
                    case EventType.AppLowMemory:
                        OnAppLowMemory(e);
                        break;
                    case EventType.AppWillEnterBackground:
                        OnAppWillEnterBackground(e);
                        break;
                    case EventType.AppDidEnterBackground:
                        OnAppDidEnterBackground(e);
                        break;
                    case EventType.AppWillEnterForeground:
                        OnAppWillEnterForeground(e);
                        break;
                    case EventType.AppDidEnterForeground:
                        OnAppDidEnterForeground(e);
                        break;
                    case EventType.DisplayEvent:
                        OnDisplayEvent(e.Display);
                        break;
                    case EventType.WindowEvent:
                        OnWindowEvent(e.Window);
                        break;
                    case EventType.SyswmEvent:
                        OnSyswmEvent(e.Syswm);
                        break;
                    case EventType.KeyDown:
                        OnKeyDown(e.Key);
                        break;
                    case EventType.KeyUp:
                        OnKeyUp(e.Key);
                        break;
                    case EventType.TextEditing:
                        OnTextEditing(e.Edit);
                        break;
                    case EventType.TextInput:
                        OnTextInput(e.Text);
                        break;
                    case EventType.KeymapChanged:
                        OnKeymapChanged(e.Key);
                        break;
                    case EventType.MouseMotion:
                        OnMouseMove(e.Motion);
                        break;
                    case EventType.MouseButtonDown:
                        OnMouseDown(e.Button);
                        break;
                    case EventType.MouseButtonUp:
                        OnMouseUp(e.Button);
                        break;
                    case EventType.MouseWheel:
                        OnMouseWheel(e.Wheel);
                        break;
                    case EventType.JoyAxisMotion:
                        OnJoyAxisMotion(e.JAxis);
                        break;
                    case EventType.JoyBallMotion:
                        OnJoyBallMotion(e.JBall);
                        break;
                    case EventType.JoyHatMotion:
                        OnJoyHatMotion(e.JHat);
                        break;
                    case EventType.JoyButtonDown:
                        OnJoyButtonDown(e.JButton);
                        break;
                    case EventType.JoyButtonUp:
                        OnJoyButtonUp(e.JButton);
                        break;
                    case EventType.JoyDeviceAdded:
                        OnJoyDeviceAdded(e.JDevice);
                        break;
                    case EventType.JoyDeviceRemoved:
                        OnJoyDeviceRemoved(e.JDevice);
                        break;
                    case EventType.ControllerAxisMotion:
                        OnControllerAxisMotion(e.CAxis);
                        break;
                    case EventType.ControllerButtonDown:
                        OnControllerButtonDown(e.CButton);
                        break;
                    case EventType.ControllerButtonUp:
                        OnControllerButtonUp(e.CButton);
                        break;
                    case EventType.ControllerDeviceAdded:
                        OnControllerDeviceAdded(e.CDevice);
                        break;
                    case EventType.ControllerDeviceRemoved:
                        OnControllerDeviceRemoved(e.CDevice);
                        break;
                    case EventType.ControllerDeviceRemapped:
                        OnControllerDeviceRemapped(e.CDevice);
                        break;
                    case EventType.FingerDown:
                        OnFingerDown(e.TFinger);
                        break;
                    case EventType.FingerUp:
                        OnFingerUp(e.TFinger);
                        break;
                    case EventType.FingerMotion:
                        OnFingerMotion(e.TFinger);
                        break;
                    case EventType.DollarGesture:
                        OnDollarGesture(e.DGesture);
                        break;
                    case EventType.DollarRecord:
                        OnDollarRecord(e.DGesture);
                        break;
                    case EventType.MultiGesture:
                        OnMultiGesture(e.MGesture);
                        break;
                    case EventType.ClipboardUpdate:
                        OnClipboardUpdate(e);
                        break;
                    case EventType.DropFile:
                        OnDropFile(e.Drop);
                        break;
                    case EventType.DropText:
                        OnDropText(e.Drop);
                        break;
                    case EventType.DropBegin:
                        OnDropBegin(e.Drop);
                        break;
                    case EventType.DropComplete:
                        OnDropComplete(e.Drop);
                        break;
                    case EventType.AudioDeviceAdded:
                        OnAudioDeviceAdded(e.ADevice);
                        break;
                    case EventType.AudioDeviceRemoved:
                        OnAudioDeviceRemoved(e.ADevice);
                        break;
                    case EventType.SensorUpdate:
                        OnSensorUpdate(e.Sensor);
                        break;
                    case EventType.RenderTargetsReset:
                        OnRenderTargetsReset(e);
                        break;
                    case EventType.RenderDeviceReset:
                        OnRenderDeviceReset(e);
                        break;
                    case EventType.UserEvent:
                        OnUserEvent(e.User);
                        break;
                    case EventType.LastEvent:
                        OnLastEvent(e);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        protected virtual void OnAppDidEnterBackground(Event @event) {
            AppDidEnterBackground?.Invoke(this, @event);
        }

        protected virtual void OnAppDidEnterForeground(Event @event) {
            AppDidEnterForeground?.Invoke(this, @event);
        }

        protected virtual void OnAppLowMemory(Event @event) {
            AppLowMemory?.Invoke(this, @event);
        }

        protected virtual void OnAppTerminating(Event @event) {
            AppTerminating?.Invoke(this, @event);
        }

        protected virtual void OnAppWillEnterBackground(Event @event) {
            AppWillEnterBackground?.Invoke(this, @event);
        }

        protected virtual void OnAppWillEnterForeground(Event @event) {
            AppWillEnterForeground?.Invoke(this, @event);
        }

        protected virtual void OnAudioDeviceAdded(AudioDeviceEvent eventADevice) {
            AudioDeviceAdded?.Invoke(this, eventADevice);
        }

        protected virtual void OnAudioDeviceRemoved(AudioDeviceEvent eventADevice) {
            AudioDeviceRemoved?.Invoke(this, eventADevice);
        }

        protected virtual void OnClipboardUpdate(Event @event) {
            ClipboardUpdate?.Invoke(this, @event);
        }

        protected virtual void OnControllerAxisMotion(ControllerAxisEvent eventCAxis) {
            ControllerAxisMotion?.Invoke(this, eventCAxis);
        }

        protected virtual void OnControllerButtonDown(ControllerButtonEvent eventCButton) {
            ControllerButtonDown?.Invoke(this, eventCButton);
        }

        protected virtual void OnControllerButtonUp(ControllerButtonEvent eventCButton) {
            ControllerButtonUp?.Invoke(this, eventCButton);
        }

        protected virtual void OnControllerDeviceAdded(ControllerDeviceEvent eventCDevice) {
            ControllerDeviceAdded?.Invoke(this, eventCDevice);
        }

        protected virtual void OnControllerDeviceRemapped(ControllerDeviceEvent eventCDevice) {
            ControllerDeviceRemapped?.Invoke(this, eventCDevice);
        }

        protected virtual void OnControllerDeviceRemoved(ControllerDeviceEvent eventCDevice) {
            ControllerDeviceRemoved?.Invoke(this, eventCDevice);
        }

        protected virtual void OnDisplayEvent(DisplayEvent eventDisplay) {
            DisplayEvents?.Invoke(this, eventDisplay);
        }

        protected virtual void OnDollarGesture(DollarGestureEvent eventDGesture) {
            DollarGesture?.Invoke(this, eventDGesture);
        }

        protected virtual void OnDollarRecord(DollarGestureEvent eventDGesture) {
            DollarRecord?.Invoke(this, eventDGesture);
        }

        protected virtual void OnDropBegin(DropEvent @event) {
            DropBegin?.Invoke(this, @event);
        }

        protected virtual void OnDropComplete(DropEvent @event) {
            DropComplete?.Invoke(this, @event);
        }

        protected virtual void OnDropFile(DropEvent eventDrop) {
            DropFile?.Invoke(this, eventDrop);
        }

        protected virtual void OnDropText(DropEvent eventDrop) {
            DropText?.Invoke(this, eventDrop);
        }

        protected virtual void OnFingerDown(TouchFingerEvent eventFinger) {
            FingerDown?.Invoke(this, eventFinger);
        }

        protected virtual void OnFingerMotion(TouchFingerEvent eventFinger) {
            FingerMotion?.Invoke(this, eventFinger);
        }

        protected virtual void OnFingerUp(TouchFingerEvent eventFinger) {
            FingerUp?.Invoke(this, eventFinger);
        }

        protected virtual void OnFirstEvent(Event @event) {
            FirstEvent?.Invoke(this, @event);
        }

        protected virtual void OnJoyAxisMotion(JoyAxisEvent eventJAxis) {
            JoyAxisMotion?.Invoke(this, eventJAxis);
        }

        protected virtual void OnJoyBallMotion(JoyBallEvent eventJBall) {
            JoyBallMotion?.Invoke(this, eventJBall);
        }

        protected virtual void OnJoyButtonDown(JoyButtonEvent eventJButton) {
            JoyButtonDown?.Invoke(this, eventJButton);
        }

        protected virtual void OnJoyButtonUp(JoyButtonEvent eventJButton) {
            JoyButtonUp?.Invoke(this, eventJButton);
        }

        protected virtual void OnJoyDeviceAdded(JoyDeviceEvent eventJDevice) {
            JoyDeviceAdded?.Invoke(this, eventJDevice);
        }

        protected virtual void OnJoyDeviceRemoved(JoyDeviceEvent eventJDevice) {
            JoyDeviceRemoved?.Invoke(this, eventJDevice);
        }

        protected virtual void OnJoyHatMotion(JoyHatEvent eventJHat) {
            JoyHatMotion?.Invoke(this, eventJHat);
        }

        protected virtual void OnKeyDown(KeyboardEvent eventKey) {
            KeyDown?.Invoke(this, eventKey);
        }

        protected virtual void OnKeymapChanged(KeyboardEvent eventKey) {
            KeymapChanged?.Invoke(this, eventKey);
        }

        protected virtual void OnKeyUp(KeyboardEvent eventKey) {
            KeyUp?.Invoke(this, eventKey);
        }

        protected virtual void OnLastEvent(Event @event) {
            LastEvent?.Invoke(this, @event);
        }

        protected virtual void OnMouseDown(MouseButtonEvent eventButton) {
            MouseDown?.Invoke(this, eventButton);
        }

        protected virtual void OnMouseMove(MouseMotionEvent eventMotion) {
            MouseMove?.Invoke(this, eventMotion);
        }

        protected virtual void OnMouseUp(MouseButtonEvent eventButton) {
            MouseUp?.Invoke(this, eventButton);
        }

        protected virtual void OnMouseWheel(MouseWheelEvent eventWheel) {
            MouseWheel?.Invoke(this, eventWheel);
        }

        protected virtual void OnMultiGesture(MultiGestureEvent eventMGesture) {
            MultiGesture?.Invoke(this, eventMGesture);
        }

        protected virtual void OnQuit(QuitEvent @event) {
            Quit?.Invoke(this, @event);
        }

        protected virtual void OnRenderDeviceReset(Event @event) {
            RenderDeviceReset?.Invoke(this, @event);
        }

        protected virtual void OnRenderTargetsReset(Event @event) {
            RenderTargetsReset?.Invoke(this, @event);
        }

        protected virtual void OnSensorUpdate(SensorEvent eventSensor) {
            SensorUpdate?.Invoke(this, eventSensor);
        }

        protected virtual void OnSyswmEvent(SysWMEvent eventSyswm) {
            SysWMEvent?.Invoke(this, eventSyswm);
        }

        protected virtual void OnTextEditing(TextEditingEvent eventEdit) {
            TextEditing?.Invoke(this, eventEdit);
        }

        protected virtual void OnTextInput(TextInputEvent eventText) {
            TextInput?.Invoke(this, eventText);
        }

        protected virtual void OnUserEvent(UserEvent eventUser) {
            UserEvent?.Invoke(this, eventUser);
        }

        protected virtual void OnWindowEvent(WindowEvent eventWindow) {
            WindowEvent?.Invoke(this, eventWindow);
        }

        #endregion
    }
}