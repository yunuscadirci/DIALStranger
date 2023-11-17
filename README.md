# DIALStranger
## What is DIAL?
Discovery and Launch (DIAL) is a protocol co-developed by Netflix and YouTube with help from Sony and Samsung.
http://www.dial-multiscreen.org/dial-protocol-specification 
## What is vulnerability?  
This is ıs a research from 2019. I found protocol doesn't cover some basic security features and most of TV vendors didnt implement protocol correctly. you can find details on [2019 original research]([DIAL Protocol Vulnerabilities and  Implementation Errors - 2019.pdf](https://github.com/yunuscadirci/DIALStranger/blob/main/DIAL%20Protocol%20Vulnerabilities%20and%20%20Implementation%20Errors%20-%202019.pdf) and [Blackhat MEA 2023 presentation]([/DIAL Protocol Vulnerabilities and Implementation Errors - 2019.pdf](https://github.com/yunuscadirci/DIALStranger/blob/main/BHMEA23_%20Dial%20Stranger%20v6.pdf) .
## Why waited 4 years
because of nature of protocol vulnerabilities - we saw for CallStranger CVE-2020-12695 - It takes forever to patc all the systems. I think waiting 4 years is enough for this vulenrability
## Are we secure now?
We are better than 2019 because:
-Netflix updated protocol 2020 and covered some holes.
-Responsible vendors updated devices.
-Browsers disabled FTP, WebRTC local IP disclosure.
We are not fully secure because
-Milions of TV's are open to Internet and by the nature of protocol, those TV's can be exploited by the hackers
-Old TVs are not updated and will not be updated
