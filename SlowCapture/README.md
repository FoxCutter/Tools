# Slow Capture

Slow Capture is as simple tool to allow streaming of programs that can't be captured by most streaming applications. All it does is take image capture of the application's client, and then displays it on it's own main window.

This simple approach works in many cases that streaming applications such as OBS or XSplit may fail on. This is because the method that it uses to capture the application is much slower then what streaming programs use, and can have some noticeable delay (10-15 fps is not uncommon).

## Why use this instead of 'Desktop Capture'

Desktop capture is a brute force solution, capturing everything on a specific part of the desktop. This does capture the program in question, but it also captures anything that might be over it (or under if the window is moved or closed). This is a huge security risk these days, so it was better to have the extra step to make sure you have full control over what is streamed. 

## What applications does Slow Capture support

In theory it should support any Windows application. It was written with the current version of Microsoft Office in mind, allowing them to be captured for streaming/recording. 

## Features

Slow Capture can match a window based on both the application name and optionally the title of the window. In addition to that, Slow Capture can also crop and resize the captured image (Just in case your streaming app of choice can't do so). 

## Requirements

.Net Framework 4.6.1 is needed for the program to work.

## Installation

The program is provided as a single EXE file, and should be able to run in place. The settings file will be written to the same location as the EXE file.

## How does it capture windows that streaming programs can't

By using a different API, as Slow Capture isn't worried about being super fast, it's not using any of the direct methods of capturing the window. Instead it is using those, it uses the "PrintWindow" API. This is a way to 'print' the contents of a window to an arbitrary device context. This is meant for a simple way to render a window to a printer, but works just fine for capturing it for streaming.

## Long Term Goals

In all honestly, the long term goal hope is that this program will become obsolete. It works as a proof of concept for using the PrintWindow API to capture a window, and hopefully streaming programs will add it as a native feature in the future. 

## Attribution

Icons made by [Freepik](https://www.flaticon.com/authors/freepik) from [Flaticon](https://www.flaticon.com/)

