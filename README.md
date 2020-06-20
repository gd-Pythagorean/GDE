### Please note:
This project is no longer maintained and is available to anyone if they want to continue on this project.
If you wish to contact the owner to get ideas on what this project was meant to be then please contact alten#4351 on [Discord](https://discordapp.com/).

<div align="center">
<h1>GD-Edit</h1>
      <a href="https://ci.appveyor.com/project/AltenGD/gd-edit"><img src="https://ci.appveyor.com/api/projects/status/rr383gfmmby75c2p?svg=true" alt="Join Discord Server"/></a>
      
[![GitHub license](https://img.shields.io/github/license/gd-edit/GDE.svg?style=flat-square)](https://github.com/gd-edit/GDE/blob/master/LICENSE) 
[![GitHub stars](https://img.shields.io/github/stars/gd-edit/GDE.svg?style=flat-square)](https://github.com/gd-edit/GDE/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/gd-edit/GDE.svg?style=flat-square)](https://github.com/gd-edit/GDE/network)
[![GitHub issues](https://img.shields.io/github/issues/gd-edit/GDE.svg?style=flat-square)](https://github.com/gd-edit/GDE/issues)
</div>

<div align="center">
    <a href="https://discord.gg/cq2FKbb"><img src="https://canary.discordapp.com/api/guilds/467885469108142100/widget.png?style=banner2" alt="Join Discord Server"/></a>
    <p><a href="https://github.com/gd-edit/GDE/releases/tag/20190731.387-2.2S">Latest Release</a> - <a href="https://youtu.be/eIRvPKWMdSk">Video Showcase</a></p>
</div>      





---

# Getting your hands onto the current unpublished version

You will need to compile the project in order to run the current version of the branch you're looking at. In other words, we do not always have an executable version of the project packed along with everything else. So, it's up to you to do some extra work in order to reach that point.

## Beginner-friendly solution

- Download GitHub for Desktop, an application that allows you to handle repositories in a user-friendly way.
- Clone this repository on your computer by clicking on the Clone or download button on the top right side of the screen, then clicking on Open in Desktop.
- Specify the folder at which you want the project to be cloned at.
- Download Visual Studio (most preferably Community 2019, **NOT** Visual Studio Code). The following versions will be compatible with this project: any (Community, Professional or Enterprise) 2015, 2017, 2019. You may also choose JetBrains Rider.
- Open the GDEdit.sln file on <folder you specified>/GDEdit, which will automatically open Visual Studio.
- (For Visual Studio:) On the top side toolbar, on the left side of the "play" button, change "Debug" to "Release" and "GDE.<whatever>" to "GDE.App".
- Start the application by clicking on the "play" button (you need to wait for the project to be built).

*It is recommended to parallelize your downloads to save time, there won't be any conflicts while downloading the required software.*

## Hardcore madman solution

- Download Git
- Open Git Bash on the directory you wish the project to be cloned into
- Do `git clone https://github.com/gd-edit/GDE.git`
- Open `GDEdit.sln` on the IDE of your choice
- Set the target build type to Release, and the target project to run to GDE.App
- Run

## Already experienced programmer solution

- Clone project, open solution, run.

# Contributing

Aside from code performance and functionality, we care about [Code Style](CodeStyle.md) as well, it's important that you check it out, as, during PR approvals, abiding to those rules is also taken into consideration. Refusing to comply with those will result in forced pushes from project admins.

# Officialities and such

GDEdit is not endorsed by RobTop. Geometry Dash is a registered trademark of RobTop Games AB.

# [Features to be added](FeaturesToBeAdded.md)
