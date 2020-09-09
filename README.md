# MadBox project : Unity


###  Time spent on this part

I have spent a total of around **8** hours on Unity, split as following :
- Around **1.5** coding the global design.
- Around **2** coding the Node.Js / Unity connection.
- Around **4.5** hours implementing differents features, bonus or not. 

###  Architectural and technical choices
I used Unity **2019.3.6f1 as requested**, and chose to have my architecture as follows : 

![alt text](https://i.stack.imgur.com/QiJAP.png "Image1")

- The **Leaderboard** GameObject is an empty one with multiple scripts attached to it, but the only one interesting to look at is the **"CustomLeaderboard"** script. The interesting variables to look at are the ones below **"LeaderBoard Customisation"** : 
    - **isAcending** determines if the players are ranked from highest to lowest or the opposite
    - **Max Player To Display** determines the number of players displayed on the leaderboard, if set to -1, the cap will be removed
    - **Number Of Decimals** determines how many decimals of the players' scores are displayed in the leaderboard 
    - The rest of the variables are the projects variable which shouldn't be changed

- The **Interface** GameObject contains the two **Canvases** we are using to use in the application :
    - **RankDisplayCanvas** is the Canvas where the Leaderboard is displayed :
    
    ![alt text](https://i.stack.imgur.com/M785c.png "Image1") 
    
    (As you can see, I'm indeed a great designer)
    
    There are **3 buttons** on this canvas, the first one is used to change **the sorting order**, the second one is used to change **the number of players displayed** and the         last one is to go back to the menu. As you might have noticed, those buttons do the exact same things as the variables in the inspector, and you're right ! I thought it was a good idea to give the player this freedom of choosing their own settings.

    - **MainMenuCanvas** is the Canvas where the Leaderboard is displayed :
    
    ![alt text](https://i.stack.imgur.com/exPDa.png "Image1")
    
    - The **Send Score** Button is used to either **create** a new entry in the database if the id is new, or **update** with a new score if the id is already existing. Be careful ! Only the last score is displayed, wether the previous was higher or not.
    - The **Delete Score** Button is used to delete a player in the database. You have to specify which player is deleted by inputting his name in the **"Your name"** input, and provide a score as well (even though the score is meaningless), otherwise the application won't let you send the delete request
    - The **Fetch ranks** Button is here to get the player database from the back-end, so you'll have to press it everytime you send a new score, as the database doesn't update on its own when you send a score (this used to be the case but it made the **Fetch** button useless, so I went back to a manual fetch)
    - The **Show ranks** takes you to the leaderboard

- And lastly, the **Client** GameObject is also an empty one, with the **ClientAPI** script attached to it, which contains all the Front-to-Back connections, and which parameters are the differents routes urls : 
![alt text](https://i.stack.imgur.com/K7Eoh.png "Image1")

### What was hard
The Unity part wasn't that hard as I'm quite familiar with the use of it. I'm however a not so good designer, so my prototypes might seem a little boring :(.

I have encountered some problem I couldn't fix, such as :
- If you input a score which is too high, the Json responses will incorrectly deal with it and it will not work as intended
- If you input a score with too many decimals, the user input gets blocked
    

### What I would do to push the project a step forward
For the leaderboard in itself, I would like to implement a **Player page** which would have tracked the evolution of the player score, and display relevant informations such as : 
- The highest score ever obtained
- The highest rank ever reached
- An evolution of the player's score trough time (maybe a graph)

### Project startup

##### Unity Version

The version I used for this project was, as I said in the intro, the **2019.3.6f1** .

##### Step by step guide

Open **Unity** and the project folder. You can then toggle and play with the variable in the **CustomLeaderboard** script I talked about earlier. Then when you're ready, you can click on **Play** and follow these steps : 
 - To **add** a player, fill out the form and click on **Send Score**  
  
    ![alt text](https://i.stack.imgur.com/2XL2s.png "Image1")

- To then **retrieve** the ranking, click on **Fetch ranks**. A cheering message will display on top of the screen. You can choose to retrieve the score without having sent one before, in that case "Fetched !" whill appear, confirming that you indeed retrieved the score.

    ![alt text](https://i.stack.imgur.com/1LHAO.png "Image1")
- After you have fetched all ranks, you can click on **Show Ranks** to go on the Leaderboard page, and the last score you sent will be highlighted 

    ![alt text](https://i.stack.imgur.com/1LHAO.png "Image1")

- You can then go back on the main page by clicking on **Back to menu**, and we can now try to update the score we just sent : input the same name and a different score, click on **Send Score** and after a manual fetch, you will see your updated score on the Leaderboard page. Note that the score will be updated wether it was higher or lower than the previous one. 

    ![alt text](https://i.stack.imgur.com/lDVcH.png "Image1")
- Lastly, you can try to **delete** the user you just created : fill the inputs with the name you want to delete and an arbitrary score, and fetch again. The score will have disappeared ! 
 
    ![alt text](https://i.stack.imgur.com/NPQqt.png "Image1")

(Note that if you didn't launch the Backend with "node app.js", Unity will fail to connect to the server and you will be issued an info log in the console)

Have fun !

### Final thoughts and bibliography

I would like to thanks everyone at MadBox who has been so nice to me and gave me a shot at joining this wonderful company.
That was a great experience !

Here are the tutorials I used :
- [Unity Tutorial]

[//]: #

   [Unity Tutorial]: https://dev.to/cemuka/making-a-rest-service-using-node-and-express-to-use-with-unity-part-1-321n
   
   

   
