//
//  SpeechRecorderViewController.m
//  SpeechToText
//
#import "SpeechRecorderViewController.h"
#import <Speech/Speech.h>

@interface SpeechRecorderViewController ()
{
    // Speech recognize
    SFSpeechRecognizer *speechRecognizer;
    SFSpeechAudioBufferRecognitionRequest *recognitionRequest;
    SFSpeechURLRecognitionRequest *recognitionURL_Request;
    
    SFSpeechRecognitionTask *recognitionTask;
    // Record speech using audio Engine
    AVAudioInputNode *inputNode;
    AVAudioEngine *audioEngine;
    NSString *LanguageCode;
    NSString *filePath;
    NSURL *url;

}
@end

@implementation SpeechRecorderViewController

- (id)init
{
    audioEngine = [[AVAudioEngine alloc] init];
    LanguageCode = @"ko-KR";
    NSLocale *local =[[NSLocale alloc] initWithLocaleIdentifier:LanguageCode];
    speechRecognizer = [[SFSpeechRecognizer alloc] initWithLocale:local];
    
    [SFSpeechRecognizer requestAuthorization:^(SFSpeechRecognizerAuthorizationStatus status) {
    //The callback may not be called on the main thread. Add an operation to the main queue to update the record button's state.
        dispatch_async(dispatch_get_main_queue(), ^{
            switch (status) {
                    
                case SFSpeechRecognizerAuthorizationStatusAuthorized: {
                    NSLog(@"SUCCESS");
                    break;
                    
                }
                case SFSpeechRecognizerAuthorizationStatusDenied: {
                    
                    NSLog(@"User denied access to speech recognition");
                    break;
                    
                }
                    
                case SFSpeechRecognizerAuthorizationStatusRestricted: {
                    NSLog(@"User denied access to speech recognition");
                    break;
                    
                }
                    
                case SFSpeechRecognizerAuthorizationStatusNotDetermined: {
                    NSLog(@"User denied access to speech recognition");
                    break;
                    
                }
            }
        });
    }];
    return self;
}

NSString* CreateNSString (const char* string)
{
    if (string)
        return [NSString stringWithUTF8String: string];
    else
        return [NSString stringWithUTF8String: ""];
}

- (void)RecognizeFile: (const char*)path {
        
    NSString * stringPath = CreateNSString(path);
    NSURL *unityURL = [NSURL fileURLWithPath:stringPath];


//   if (speechRecognizer.isAvailable) {
//
//      // The recognizer is not available right now
//      return
//
//     }

       SFSpeechURLRecognitionRequest *recognitionURL_Request = [[SFSpeechURLRecognitionRequest alloc] initWithURL:unityURL];
    
        recognitionURL_Request.shouldReportPartialResults = YES;
        recognitionTask =[speechRecognizer recognitionTaskWithRequest:recognitionURL_Request resultHandler:^(SFSpeechRecognitionResult * _Nullable result, NSError * _Nullable error)
       {
     
           if (result) {
               
               NSArray *results = result.transcriptions;
               NSString *combined = [results componentsJoinedByString:@"~"];
//               for (NSString* result in results)
//               {
//                   //result.
//                   //NSString *result_part = rformattedString;
              //     UnitySendMessage("SpeechToText", "onResultsArray", [results.description UTF8String]);
//               }
               
//
//               for (int i = 0; i < [results count]; i++)
//               {
//                   NSString *s = [results objectAtIndex:i];
//                   UnitySendMessage("SpeechToText", "onResultsArrayCallback", [s UTF8String]);
//               }
//
//
//               NSString *s = @"Some string";
//               const char *c = [s UTF8String];
               
//               NSString *transcriptText = result.bestTranscription.formattedString;
//
//               NSString *x = @"|";
//               NSString * finaltext = [NSString stringWithFormat:@"%@/%@/%@", transcriptText, x, combined];
               
               //NSString * tryThis = strcat(char , const char *__s2)
               
              // NSLog(@"STARTRECORDING RESULT: %@", transcriptText);
               
               
               
               if (result.isFinal) {
                   
                   UnitySendMessage("SpeechToText", "onResultsArray", [combined UTF8String]);

                  // UnitySendMessage("SpeechToText", "onResults", [transcriptText UTF8String]);
                   //UnitySendMessage("SpeechToText", "onResults", [finaltext UTF8String]);
               }
           }
           else {
               [audioEngine stop];
               recognitionTask = nil;
               recognitionRequest = nil;
               
               
               UnitySendMessage("SpeechToText", "onResults", "ERROR :((((");
               
               
               NSLog(@"STARTRECORDING RESULT NULL");
           }
       }];
   }
- (void)SettingSpeech: (const char *) _language
{
    LanguageCode = [NSString stringWithUTF8String:_language];
    NSLocale *local =[[NSLocale alloc] initWithLocaleIdentifier:LanguageCode];
    speechRecognizer = [[SFSpeechRecognizer alloc] initWithLocale:local];
    UnitySendMessage("SpeechToText", "onMessage", "Setting Success");
}

// recording
- (void)startRecording {
    if (!audioEngine.isRunning) {
        [inputNode removeTapOnBus:0];
        if (recognitionTask) {
            [recognitionTask cancel];
            recognitionTask = nil;
        }
                
        AVAudioSession *session = [AVAudioSession sharedInstance];
        [session setCategory:AVAudioSessionCategoryPlayAndRecord withOptions:AVAudioSessionCategoryOptionDefaultToSpeaker|AVAudioSessionCategoryOptionMixWithOthers error:nil];
        [session setActive:TRUE error:nil];
        
        inputNode = audioEngine.inputNode;
        
        recognitionRequest = [[SFSpeechAudioBufferRecognitionRequest alloc] init];
        
        //how to reverse audiobuffer and give the confidence of recognition back
        //custom clipillä eka toi regoc --- miten saa ton
        //AVAudioSessionCategoryRecord
        //The category for recording audio; this category silences playback audio.
        
        recognitionRequest.shouldReportPartialResults = YES;
        recognitionTask =[speechRecognizer recognitionTaskWithRequest:recognitionRequest resultHandler:^(SFSpeechRecognitionResult * _Nullable result, NSError * _Nullable error)
        {
            
            if (result) {
                
                NSString *transcriptText = result.bestTranscription.formattedString;
                NSLog(@"STARTRECORDING RESULT: %@", transcriptText);
                if (result.isFinal) {
                    UnitySendMessage("SpeechToText", "onResults", [transcriptText UTF8String]);
                }
                
            }
            else {
                
                [audioEngine stop];
                recognitionTask = nil;
                recognitionRequest = nil;
                UnitySendMessage("SpeechToText", "onResults", "nil");
                NSLog(@"STARTRECORDING RESULT NULL");
                
            }
        }];

        AVAudioFormat *format = [inputNode outputFormatForBus:0];
        
        [inputNode installTapOnBus:0 bufferSize:1024 format:format block:^(AVAudioPCMBuffer * _Nonnull buffer, AVAudioTime * _Nonnull when) {
        [recognitionRequest appendAudioPCMBuffer:buffer];
            
        }];
        [audioEngine prepare];
        NSError *error1;
        [audioEngine startAndReturnError:&error1];

        if (error1.description) {
            NSLog(@"errorAudioEngine.description: %@", error1.description);
        }
    }
}

- (void)stopRecording {
    if (audioEngine.isRunning) {
        [inputNode removeTapOnBus:0];
        [audioEngine stop];
        [recognitionRequest endAudio];
    }
}

@end
extern "C"{
    SpeechRecorderViewController *vc = nil;
    
    SpeechRecorderViewController *getVc() {
        if (vc == nil) {
            vc = [[SpeechRecorderViewController alloc] init];
        }
        
        return vc;
    }
    
    void _TAG_startRecording(){
        SpeechRecorderViewController *pVc = getVc();
        [pVc startRecording];
    }
    void _TAG_stopRecording(){
        SpeechRecorderViewController *pVc = getVc();
        [pVc stopRecording];
    }
    void _TAG_SettingSpeech(const char * _language){
        
        SpeechRecorderViewController *pVc = getVc();
        [pVc SettingSpeech:_language];
        
    }
    void _TAG_RecognizeFile(const char * path){
        
        SpeechRecorderViewController *pVc = getVc();
        [pVc RecognizeFile:path];
    }
}
