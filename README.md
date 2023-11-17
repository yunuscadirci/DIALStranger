# DIALStranger
## What is DIAL?
Discovery and Launch (DIAL) is a protocol co-developed by Netflix and YouTube with help from Sony and Samsung.
http://www.dial-multiscreen.org/dial-protocol-specification 
## What is vulnerability?  
This is a research from 2019. I found protocol doesn't cover some basic security features and most of TV vendors didnt implement protocol correctly. You can find details on [2019 original research]([DIAL Protocol Vulnerabilities and  Implementation Errors - 2019.pdf](https://github.com/yunuscadirci/DIALStranger/blob/main/DIAL%20Protocol%20Vulnerabilities%20and%20%20Implementation%20Errors%20-%202019.pdf) and [Blackhat MEA 2023 presentation]([/DIAL Protocol Vulnerabilities and Implementation Errors - 2019.pdf](https://github.com/yunuscadirci/DIALStranger/blob/main/BHMEA23_%20Dial%20Stranger%20v6.pdf) .



https://github.com/yunuscadirci/DIALStranger/assets/7267858/80812824-f924-4220-8ee3-6600a2955e7f



## Why waited 4 years
Because of nature of protocol vulnerabilities - we saw for CallStranger CVE-2020-12695 - it takes forever to patc all the systems. I think waiting 4 years is enough for this vulnerability
## Are we secure now?
We are better than 2019 because:
- Netflix updated protocol 2020 and covered some holes.
- Responsible vendors updated devices.
- Browsers disabled FTP, WebRTC local IP disclosure. These were used for finding and exploiting local devices

We are not fully secure because
- Old TVs are not updated and will not be updated 
- Even we dont know local IP, we can spray with ajax 
## DIAL is a local protocol
Absolutely no. Milions of TV's are open to Internet and by the nature of protocol, those TV's can be exploited by the hackers. They can be used for propaganda and profit
