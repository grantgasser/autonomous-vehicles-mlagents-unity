# Vehicle Agent

## Vehicle Mechanics
The bases of the vehicle is from [Unity's Vehicle Tool]() Asset. The family car was used and altered to conform to `Agent` From ML-Agents. The car consists of a chassis model and 4 wheel using the Wheel Collider object. We added a custom script to the wheel object that uses a ray cast to determine the position on road. The following are the parameters used in our code (All values mentioned are constant for training, some values are not mentioned because they are training specific or defaulted from Vehicle Tools):
* Script: `VehicleAgent.cs` (`Agent`)
  * **Max Wheel Angle**: `30`
  * **Torque**(Constant): `50`
  * **Brake**: `None`
  * **Road Guide Offset**: `15`
* Script: `CollidingWheel.cs` (`MonoBehavior`)
  * **Side**: `[Driver, Passenger]`

![vehicle](/docs/images/vehicle_ray_casts.png) 

## Observations (Environment State)
Every decision interval, observations from the environment are reported to the "brain" (Behavior Parameters Script from ML-Agents). These observations determine the state of the environment.
* Wheel Angle, normalized `[-1, 1]`
* Front Distance to Center, normalized `[-1, 1]`
  * In respect to the font axel, center of the right lane is `0`.
*  Back Distance to Center, normalized `[-1, 1]`
   *  In respect to the back axel, center of the right lane is `0`.
* Z axes velocity from the vehicle's rigidbody
* X axes velocity from the vehicle's rigidbody

## Action

### Continuous Reward System (vs01)
For our initial iteration, there is only one action given from the model, the angle of the wheels. This is given as a continuous value between -1 and 1, then translated to represent the the -30, 30 range of the wheels.

### Discrete Reward System (vs02 & vs03)
Straight is under represented in the continuous action space. so we create a new experiment with a discrete classification action with 3 classes:
* **-1 degree**: move the wheel 0.25 degree to the left
* **0 degree**: continue with the same wheel angle
* **1 degree**: move the wheel 0.25 degree to the right

We also gave a slight reward for driving straight. This provided a much smoother control than the continuous reward and resulted in a model we can use for our CNN model.

## Reward
After a action is taken a reward is determined based on that action. For each decision interval, the a reward with the range -1 to 1 is given.

### Lane positioning
Front and back axel position relative to the lane guides are normalized from the raw distance from the 4 `CollidingWheel` Behavior objects. `0` represents the center of the lane, `-1` would be colliding the left lane guide and `1` would be colliding with the right lane guide. Rewards are given by the following logic:
* given `|frontLaneOffset| < 0.2 AND |backLaneOffset| < 0.2`
  * `r = (1 - |frontLaneOffset|) * 0.25) + ((1 - |backLaneOffset|) * 0.25)`
* given `frontLaneOffset >= 0.2 AND backLaneOffset >= 0.2`
  * `r = -(|frontLaneOffset| * 0.25) + (|backLaneOffset| * 0.25))`
* given `frontLaneOffset >= -0.2 AND backLaneOffset >= -0.2`
  * `r = 2 * -(|frontLaneOffset| * 0.25) + (|backLaneOffset| * 0.25))`

### Temination
* if the vehicle reaches the end box:
  * `r = 1`
* if the vehicle runs off the road, flips, or falls off the terrain:
  * `r = -1`
