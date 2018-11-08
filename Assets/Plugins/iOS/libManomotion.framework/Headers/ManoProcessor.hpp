//
//  ManoProcessor.h
//  ManoSDK
//
//  Created by Mhretab on 26/04/16.
//  Copyright © 2016 ManoMotion. All rights reserved.
//

#ifndef ManoProcessor_h
#define ManoProcessor_h

#define WEAK_IMPORT __attribute__((weak))
#define EXTERN __declspec(dllexport)
#define MAX_CONTOUR_POINTS 200
#define RGB_FORMAT 0
#define PORTRAIT_ORIENTATION 1
#define INV_PORTRAIT_ORIENTATION 3
#define LANDSCAPE_ORIENTATION 2
#define INV_LANDSCAPE_ORIENTATION 0
#include "opencv2/opencv.hpp"
#include <string>
#include <stdio.h>
#include  <ios>

#include "public_structs.h"
//#include "catch.hpp"
using namespace cv;
using namespace std;


enum AddOn
{
    ADD_ON_DEFAULT = 0,
    ADD_ON_ARKIT = 1,
    ADD_ON_CORE = 1,
    ADD_ON_VUFORIA = 3
};


using namespace std;

/*
 * This dataset collects all the useful information to be provided to the developer,
 *  It contains not only the data retrived from the the database but also the dynamic gesture, roi size and position and binary
 */

#define ENTRY_POINT __attribute__ ((visibility ("default")))
#define PROTECTED __attribute__ ((visibility ("protected")))
#define INTERNAL __attribute__ ((visibility ("internal")))
//#define HIDDEN __attribute__ ((visibility ("hidden")))

extern "C"  {
   
#ifdef _LOG_MEASUREMENT_ON_
    vector<int> pre_process_values;
    vector<int> return_info_values;
    vector<int> whole_process_frame_values;
#endif
    /*
     * This method must be always called before anything in order to initialize all the necessary vbles
     */
    
    ENTRY_POINT int init(char * serial_key);
    
   // ENTRY_POINT void processFrame(HandInfo* hand_info0,HandInfo* hand_info1,Session * session_info);
    ENTRY_POINT void processFrame(HandInfo *hand_info0,   Session *manomotion_session);
    
#ifdef _LOG_MEASUREMENT_ON_
    ENTRY_POINT void stop();
#endif
    
    ENTRY_POINT void  setFrameArray (void * data);
    
    
    ENTRY_POINT void  setResolution(int width, int height);
    
    
    
}
#endif /* ManoProcessor_h */
