namespace ConsoleFramework
{
    // https://github.com/ccourson/LewanSoul-xArm
    public enum RobotCommand
    { //                     Dec    *   Hex Tx; Rx
      //--------------------+-----++-+----+--------------------------------------------------------
        MultiServoMove = 3, //* 0x03 (byte)count (ushort)milliseconds { (byte)servo (ushort)position[FF00=null] }

        GroupDownload = 5, //* 0x05 (byte)cmd (byte)group [parameters]
                           //  - cmd=1: parameters={ (byte)count[mod(255)] }[extends by byte as needed]; (byte)cmd 00
                           //  - cmd=2: parameters=00 (byte)count 00                                   ; (byte)cmd 00
                           //  - cmd=3: parameters=00 00          [255 actions]                        ; (byte)cmd 00
                           //  - cmd=3: parameters=00 00 01       [512 actions]                        ; (byte)cmd 00
                           //  - cmd=3: parameters=00 00 01 02    [765 actions]                        ; (byte)cmd 00
                           //  - cmd=3: parameters=00 00 01 02 03 [1,020 actions]                      ; (byte)cmd 00
                           //  - cmd=4: parameters=(byte)block (byte)index 0B (ushort)milliseconds { (byte)servo (ushort)position } 
                           //                                                                          ; (byte)cmd 00
                           //  - cmd=5: parameters= -none-                                             ; (byte)cmd 00
                           //  * A group may contain 4 blocks of 255 actions or 1,020 actions.        

        GroupRun = 6, //* 0x06 (byte)group (ushort)count[0=continuous]
        GroupStop = 7, //  0x07 -none-
        GroupErase = 8, //* 0x08 (byte)group[255=all]; [empty response]

        GroupSpeed = 11, //  0x0b (byte)group (ushort)percentage

        GetBatteryVoltage = 15, //  0x0f -none-; (ushort)millivolts

        MultiServoOff = 20, //* 0x14 (byte)count { (byte)servo }
        ServoPositionRead = 21, //* 0x15 (byte)count { (byte)servo }; (byte)count { (byte)servo (ushort)position }
        ServoOffsetWrite = 22, //* 0x16 (byte)count { (byte)servo }
        ServoOffsetRead = 23, //* 0x17 (byte)count { (byte)servo }; (byte)count { (byte)servo (sbyte)offset }
        ServoOffsetAdjust = 24, //* 0x18 (byte)servo (short)offset

        ServoSpeed = 26, //* 0x1a (byte)servo (byte)mode[0=servo,1=motor] (ushort)milliseconds
        BusServoInfoWrite = 27, //* 0x1b (byte)servo (ushort)position_min (ushort)position_max (ushort)millivolts_min (ushort)millivolts_max (ushort)temp_max (byte)led_status[0=enable,1=disable] (byte)led_warning[bits:0=overheat,1=overvoltage,2=overposition]
        BusServoInfoRead = 28  //* 0x1c -none-; (byte)id (ushort)position_min (ushort)position_max (ushort)millivolts_min (ushort)millivolts_max (ushort)temp_max (byte)led_status[0=enable,1=disable] (byte)led_warning[bits:0=overheat,1=overvoltage,2=overposition] (byte)offset (ushort)position (byte)temp (ushort)millivolts
    }
}
