# Unity Development
Details on the unity development environment used for the simulation. Our simulation project can be found under `vehicle-sim-env/` and has been developed in [Unity]() version `2019.3.0f6`.

## Unity Packages
* [The Unity Machine Learning Agents Toolkit](https://github.com/Unity-Technologies/ml-agents)
  * version: `0.13.1`
  * an open-source Unity plugin that enables games and simulations to serve as environments for training intelligent agents.
* [Vehicle Tools](https://assetstore.unity.com/packages/essentials/tutorial-projects/vehicle-tools-83660)
  * version: `1.9`
  * Asset by Unity from Unity's Asset Store. 3 basic vehicle models created using Unity's wheel collider. The family car was modified for our vehicle
* [Road Architect](https://github.com/MicroGSD/RoadArchitect)
  * version: `1.7`
  * Open source Unity package used to create roads
* [Terrin Tools Sample Asset Pack](https://assetstore.unity.com/packages/2d/textures-materials/terrain-tools-sample-asset-pack-145808)
  * version: `1.0`
  * Asset by Unity from Unity's Asset Store. Includes some helpful tools for creating terrains.

## Package Modifications
* Road Architect
  * **Lane Markers** (see [RL Environment](rl_environment.md))
    * Used the side walk prefab from Road Architect to construct a custom invisible wall for lane guidance in the reinforcement learning model.
* Vehicle Tools
  * **Family Car**: see [Vehicle Agent](vehicle_agent.md)
    * The base model of our vehicle. We altered the included vehicle script to create our agent.
    * Altered the prefab to include wheel trigger collision boxes with ray casting for road positioning.


## Project Creation
**Note**: The following has already been completed for the `vehicle-sim-env` project. Given the active development of ML-Agents, we are documenting the steps to create our project.
1. Create new Unity Project
2. Clone Machine Learning Agents Toolkit
3. Copy `ML-Agents/UnitySDK/ML-Agents` => `new_proj/Assets/`
4. Import Road Architect `.unityProject`
5. Import Vehicle Tools
6. Import Terrain Tools

