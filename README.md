# SuperNovaProject

## A scene chasing mini-game based on the ragdoll system

### AnimationBody.cs

Synchronize motion of animated bones to physical bones.

### Behaviour.cs

Handle user input and perform corresponding movement of the physical model (WASD movement, jumping).

### CameraFollow.cs

Keeps the camera tracking the character at all times while synchronizing the camera's rotation relative to the character based on the horizontal input of the user's mouse.

### GrabBehaviour.cs

Control the grabbing behavior of the hands, including grabbing walls and grabbing limbs of other NPCs.

### IKSetter.cs

Execute grab action when user long press the left mouse button.

### JointRotation.cs

Rotate a joint from a start position to a target position.

### Model.cs

Get the instance of model.

### PhysicsBody.cs

Modify the orientation of the physical model based on user input.

### Ragdoll.cs

Configure the component and initially set the spring torque and damping of the model's joints.

### Utils.cs

Some self-defined helper functions.

### FinalIK & PuppetMaster

External package for hand IK configuration and character model collision box adjustment.
