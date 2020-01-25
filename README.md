# Advanced Deep Learning Project

## Authors
* [Grant Gasser](https://www.linkedin.com/in/grantgasser/)
* [Blaine Rothrock](https://www.linkedin.com/in/brothrock/)

## DonkeyCar Report & Thoughts
* [DonkeyCar Docs](https://docs.donkeycar.com/)
* See `exploration/` for report on DonkeyCar

## Unity Environment + ML Agents  
* [mlagents repo](https://github.com/Unity-Technologies/ml-agents)
* [mlagents paper](https://arxiv.org/pdf/1809.02627.pdf)

## Self-Driving Car Research
### Tesla
* Sensors:
Eight surround **cameras** provide 360 degrees of visibility around the car at up to 250 meters of range. Twelve updated **ultrasonic sensors** complement this vision, allowing for detection of both hard and soft objects at nearly twice the distance of the prior system. A forward-facing **radar** with enhanced processing provides additional data about the world on a redundant wavelength that is able to see through heavy rain, fog, dust and even the car ahead. **Sonar devices** (like parking sensors) detect objects in close range.
  - Ultrasonic: usually short distance
  - Radar: like an echo but with radio waves (long wave lengths on electromagnetic spectrum) instead of sound waves
  - [Autonomy pres](https://www.youtube.com/watch?v=HM23sjhtk4Q)
  - Elon Musk: "Lidar is a fools errand. Anyone relying on lidar is doomed. Expensive, unecessary.
  - Karpathy: Can annotate video using a sensor like radar ("this car you're seeing in front of you in this box is 17m away"), train neural network on that
  - self-supervised way of learning depth with video: just be consistent in all predictions (no targets)
  - images/video contain are much more information rich (lidar can't tell you the difference between plastic bag or tire)
  - radar can go thru fog, dust, snow, rain, etc. where lidar isn't great; lidar (_light detection and ranging_) uses ultraviolet, visible, or near infrared light to image objects
  - detection is best outside/away from visual spectrum
  - Elon: high precision GPS maps are a really bad idea
  
### NVIDIA Self-driving software
* [Conv Net](https://devblogs.nvidia.com/deep-learning-self-driving-cars/) approach: imitate driver
* Input: Images, Targets: (wheel angle)

## Lidar Approach
* Hardly uses machine learning, mostly about signal processing, integration, and planning

## Tentative Plan
* Add randomness to environment to avoid overfitting or have many tracks
* Could do curriculum training (straight line-> one curve -> many curves...); difficulty starts easy and increases
* Baseline goal: lane keeping with RL agent, after trained, collect images (1-8 cameras) with associated wheel angle and acceleration
* 2nd goal: Conv Net (TF); Input: Images collected from RL agent and (our driving), Targets: (wheel angle, acceleration, etc.)
* Lofty goal: Use lidar (or radar) and add several RL agents 

## Questions 
* How to avoid overfitting (memorizing track)? How many different tracks do we need? Can we add randomness to the tracks in some way?

