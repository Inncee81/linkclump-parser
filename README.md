## linkclump-parser

linkclump-parser is a clipboard parsing tool for saving all your web scraped urls or data to file.

Ok, for those who are into web scaping we have come across many tools and one such tool is [Linkclump](https://github.com/benblack86/linkclump). Linkclump is a chorme and firefox plugin where user can use mouse to copy or bookmark urls. But issue arise when we have to copy/paste a large set of urls, that's a lot of copy paste and more importantly such a manual process.

So, I have written this little tool in C# which address this issue. This tool copies all the links into its memory and filters duplicate links before saving into text file.

## Installation
  
  `Just download or clone the folder. Assuming you already have visual studio installed, now once project loaded just run and have fun.`

## FAQ
- Does the linkclump-parser has a limit on how much links in its memory

  `Yes, currently autosave happens if the urls are greater than equal to 100. ex: rawstrings.Count > 100`

- Where does the final links file saved

  `It should save in your project folder`

- Does this app supports Linux, OSX

  `I am not sure, I haven't tested on those OS`
  
- Does the linkclump-parser runs on its own and copy the urls

  `No, this a clipboard parser tool. So you will need linkclump or any such tool to copy all the links to clipboard and once its in            clipboard linkclump-parser will trigger and saves the links to the file`

## Licence
  linkclump-parser is licensed under a MIT license.

