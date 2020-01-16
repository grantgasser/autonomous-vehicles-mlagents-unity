
from donkeycar.parts.controller import Joystick, JoystickController


class xbox_one(Joystick):
    #An interface to a physical joystick available at /dev/input/js0
    def __init__(self, *args, **kwargs):
        super(xbox_one, self).__init__(*args, **kwargs)

            
        self.button_names = {
            0x13b : 'start',
            0x13a : 'select',
            0x134 : 'Y',
            0x131 : 'B',
            0x130 : 'A',
            0x133 : 'X',
            0x137 : 'R1',
            0x136 : 'L1',
            0x13e : 'RJ1',
            0x13d : 'LJ1',
        }


        self.axis_names = {
            0x3 : 'RightJS',
            0x11 : 'DPAD',
        }



class xbox_oneController(JoystickController):
    #A Controller object that maps inputs to actions
    def __init__(self, *args, **kwargs):
        super(xbox_oneController, self).__init__(*args, **kwargs)


    def init_js(self):
        #attempt to init joystick
        try:
            self.js = xbox_one(self.dev_fn)
            self.js.init()
        except FileNotFoundError:
            print(self.dev_fn, "not found.")
            self.js = None
        return self.js is not None


    def init_trigger_maps(self):
        #init set of mapping from buttons to function calls
            
        self.button_down_trigger_map = {
            'select' : self.toggle_mode,
            'Y' : self.erase_last_N_records,
            'R1' : self.emergency_stop,
            'A' : self.increase_max_throttle,
            'B' : self.decrease_max_throttle,
            'L1' : self.toggle_constant_throttle,
            'X' : self.toggle_manual_recording,
        }


        self.axis_trigger_map = {
            'RightJS' : self.set_steering,
            'DPAD' : self.set_throttle,
        }


