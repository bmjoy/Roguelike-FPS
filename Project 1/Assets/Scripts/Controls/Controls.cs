using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using InputController;


// Actions needing a key binding.
public enum GameButton
{
    Menu,
    Fire,
    Aim,
    Reload,
    Jump,
    Run,
    Crouch,
    Interact,
}

// Actions needing an axis binding.
public enum GameAxis
{
    LookX,
    LookY,
    MoveX,
    MoveY,
}


/*
 * Stores and maintains user constrols.
 */
[RequireComponent(typeof(ControlsEarlyUpdate))]
public class Controls : MonoBehaviour
{
    private static Dictionary<GameButton, BufferedButton> m_buttons;
    public static Dictionary<GameButton, BufferedButton> Buttons
    {
        get { return m_buttons; }
    }

    private static Dictionary<GameAxis, BufferedAxis> m_axis;
    public static Dictionary<GameAxis, BufferedAxis> Axis
    {
        get { return m_axis; }
    }

    private static bool m_isMuted = false;
    public static bool IsMuted
    {
        get { return m_isMuted; }
        set { m_isMuted = value; }
    }

    private void Awake()
    {
        loadDefaultControls();
    }

    /*
     * Needs to run at the end of every FixedUpdate frame to handle the input buffers.
     */
    private void FixedUpdate()
    {
        foreach (BufferedButton button in m_buttons.Values)
        {
            button.RecordFixedUpdateState();
        }
        foreach (BufferedAxis axis in m_axis.Values)
        {
            axis.RecordFixedUpdateState();
        }
    }

    /*
     * Needs to run at the start of every Update frame to buffer new inputs.
     */
    public void EarlyUpdate()
    {
        foreach (BufferedButton button in m_buttons.Values)
        {
            button.RecordUpdateState();
        }
        foreach (BufferedAxis axis in m_axis.Values)
        {
            axis.RecordUpdateState();
        }
    }

    /*
     * Clears the current controls and replaces them with the default set.
     */
    public static void loadDefaultControls()
    {
        m_buttons = new Dictionary<GameButton, BufferedButton>();
        
        m_buttons.Add(GameButton.Menu, new BufferedButton(false, new List<IButtonSource>
        {
            new KeyButton(KeyCode.Escape),
            new JoystickButton(GamepadButton.Start)
        }));
        m_buttons.Add(GameButton.Fire, new BufferedButton(true, new List<IButtonSource>
        {
            new KeyButton(KeyCode.Mouse0),
            new JoystickButton(GamepadButton.RTrigger)
        }));
        m_buttons.Add(GameButton.Aim, new BufferedButton(true, new List<IButtonSource>
        {
            new KeyButton(KeyCode.Mouse1),
            new JoystickButton(GamepadButton.LTrigger)
        }));
        m_buttons.Add(GameButton.Reload, new BufferedButton(true, new List<IButtonSource>
        {
            new KeyButton(KeyCode.R),
            new JoystickButton(GamepadButton.Y)
        }));
        m_buttons.Add(GameButton.Jump, new BufferedButton(true, new List<IButtonSource>
        {
            new KeyButton(KeyCode.Space),
            new JoystickButton(GamepadButton.A)
        }));
        m_buttons.Add(GameButton.Run, new BufferedButton(true, new List<IButtonSource>
        {
            new KeyButton(KeyCode.LeftShift),
            new JoystickButton(GamepadButton.LStick)
        }));
        m_buttons.Add(GameButton.Crouch, new BufferedButton(true, new List<IButtonSource>
        {
            new KeyButton(KeyCode.C),
            new JoystickButton(GamepadButton.B)
        }));
        m_buttons.Add(GameButton.Interact, new BufferedButton(true, new List<IButtonSource>
        {
            new KeyButton(KeyCode.F),
            new JoystickButton(GamepadButton.X)
        }));


        m_axis = new Dictionary<GameAxis, BufferedAxis>();
        
        m_axis.Add(GameAxis.LookX, new BufferedAxis(true, new List<IAxisSource>
        {
            new MouseAxis(MouseAxis.Axis.MouseX),
            new JoystickAxis(GamepadAxis.RStickX, 2.0f, 1.0f)
        }));
        m_axis.Add(GameAxis.LookY, new BufferedAxis(true, new List<IAxisSource>
        {
            new MouseAxis(MouseAxis.Axis.MouseY),
            new JoystickAxis(GamepadAxis.RStickY, 2.0f, 1.0f)
        }));
        m_axis.Add(GameAxis.MoveX, new BufferedAxis(true, new List<IAxisSource>
        {
            new KeyAxis(KeyCode.A, KeyCode.D),
            new JoystickAxis(GamepadAxis.LStickX, 1.0f, 1.0f)
        }));
        m_axis.Add(GameAxis.MoveY, new BufferedAxis(true, new List <IAxisSource>
        {
            new KeyAxis(KeyCode.S, KeyCode.W),
            new JoystickAxis(GamepadAxis.LStickY, 1.0f, 1.0f)
        }));
    }

    /*
     * Returns true if any of the relevant keyboard or joystick buttons are held down.
     */
    public static bool IsDown(GameButton button)
    {
        BufferedButton bufferedButton = m_buttons[button];
        return !(m_isMuted && bufferedButton.CanBeMuted) && bufferedButton.IsDown();
    }

    /*
     * Returns true if a relevant keyboard or joystick key was pressed since the last appropriate update.
     */
    public static bool JustDown(GameButton button)
    {
        BufferedButton bufferedButton = m_buttons[button];
        bool isFixed = (Time.deltaTime == Time.fixedDeltaTime);
        return !(m_isMuted && bufferedButton.CanBeMuted) && (isFixed ? bufferedButton.JustDown() : bufferedButton.VisualJustDown());
    }

    /*
     * Returns true if a relevant keyboard or joystick key was released since the last appropriate update.
     */
    public static bool JustUp(GameButton button)
    {
        BufferedButton bufferedButton = m_buttons[button];
        bool isFixed = (Time.deltaTime == Time.fixedDeltaTime);
        return !(m_isMuted && bufferedButton.CanBeMuted) && (isFixed ? bufferedButton.JustUp() : bufferedButton.VisualJustUp());
    }

    /*
     * Returns the average value of an axis from all Update frames since the last FixedUpdate.
     */
    public static float AverageValue(GameAxis axis)
    {
        BufferedAxis bufferedAxis = m_axis[axis];
        return (m_isMuted && bufferedAxis.CanBeMuted) ? 0 : bufferedAxis.AverageValue();
    }

    /*
     * Returns the cumulative value of an axis from all Update frames since the last FixedUpdate.
     */
    public static float CumulativeValue(GameAxis axis)
    {
        BufferedAxis bufferedAxis = m_axis[axis];
        return (m_isMuted && bufferedAxis.CanBeMuted) ? 0 : bufferedAxis.CumulativeValue();
    }
}