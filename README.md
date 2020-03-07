# LewanSoul robotic xArm

Using robotic arm with 6 servos is hard to control. All angle calculations must be done in mind, and applied using controller. It can be solved with some math functions. Bellow is provided math behind moving arm up or down in two axis mode.

# Arm structure

<p align="center">
  <img src="./docs/images/shape_1.png" width="20%" height="20%">
</p>
<br/>

From side robotic arm can be seen as in image. Moving arm up or down, closer or further requires calculating angles of servos. What can be done is calculate angles based on it's current position and delta shift in vertical and horizontal axes. Its enougth to use for calculations ABCD quadrilateral since some angles and sides are known, also it is know what position shift.

<img src="./docs/images/shape_2.png" width="20%" height="20%">
<img src="./docs/images/shape_3.png" width="20%" height="20%">
