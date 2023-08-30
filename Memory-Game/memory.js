/*
    UI Section
*/
const player_names_debug = ["Ronaldo", "Messi"];

let menu;
let players_list;
let player_name;

let gameplay;

function init(){
    menu = document.getElementById("menu");
    gameplay = document.getElementById("gameplay");
    players_list = document.getElementById("players");
    player_name = document.getElementById("player_name");
    player_names_debug.forEach( (n) => addPlayer2(n) );
    showMenu();
}

function showMenu(){
    menu.style.display = "initial";
    gameplay.style.display = "none";
}

function showGameplay(){
    menu.style.display = "none";
    gameplay.style.display = "initial";
}

/*
    Menu section
*/

const player_names = [];

function addPlayer(){
    addPlayer2(player_name.value);
}

function addPlayer2(name){
    if (player_names.length >= 4)
        alert("Player limit has been reached");
    else if (!name.match("^[a-zA-Z0-9_]{2,16}$"))
        alert("Invalid player name");
    else if (player_names.indexOf(name) != -1)
        alert("Player " + name + " already exists.");
    else{
        player_name.value = "";
        player_names.push(name);

        var playerdiv = document.createElement("div");
        var playertext = document.createElement("p");
        var playerdelete = document.createElement("button");

        playerdelete.className = "remove_player";
        playertext.className = "player_name";
        playerdiv.className = "player_div"

        playertext.textContent = name;

        playerdiv.appendChild(playertext);
        playerdiv.appendChild(playerdelete);

        playerdelete.onclick = ( (n, pd) => function(){
            var index = player_names.indexOf(n);
            player_names.splice(index, 1);
            players_list.removeChild(pd);
        } )(name, playerdiv);

        players_list.appendChild(playerdiv);
    }
}

function startGame() {
    if (player_names.length < 1){
        alert("Not enough players.");
        return;
    }

    try{
        var fsize = document.querySelector('input[name="field_size"]:checked').value.split(",");
        var set = document.querySelector('input[name="card_set"]:checked').value;

        initGame(parseInt(fsize[0]), parseInt(fsize[1]), set, player_names);
        showGameplay();
    }catch(TypeError){
        alert("Select card set & field size.");
    }
    
}

/*
    Game section
*/
const game = {
    players: undefined,
    width: undefined,
    heigth: undefined,
    turn: undefined,
    card_set: undefined,
    field: undefined
}

function initGame(width, heigth, set, names){
    game.players = [];
    game.turn = {
        player: Math.floor(Math.random() * names.length),
        first_selection: undefined,
        second_selection: undefined
    };
    game.width = width;
    game.heigth = heigth;
    game.card_set = set;
    game.field = createField(width, heigth);

    names.forEach( (n) => game.players.push({
        name: n,
        score: 0
    }));

    console.log(game);
    initUI();
}

function initUI(){
    invalidateTurn();
    initScore();
    initCards();
}

function initScore(){
    var d = document.getElementById("score_info");
    d.replaceChildren();

    for (var i = 0 ; i < game.players.length ; i++){
        var e = document.createElement("div");
        var f = document.createElement("div");
        var g = document.createElement("div");

        e.className = "player_score_div";
        f.className = "player_score_name";
        g.className = "player_score";

        g.textContent = game.players[i].score;
        g.id = "player_score_" + i;
        f.textContent = game.players[i].name;

        e.appendChild(f);
        e.appendChild(g);
        d.appendChild(e);
    }
}

function initCards(){
    var area = document.getElementById("cards_area");
    area.replaceChildren();

    var row, scene, card, card_front, card_back;

    for (var i = 0 ; i < game.heigth ; i++){
        row = document.createElement("div");  
        row.className = "cards_area_row";

        for (var j = 0 ; j < game.width ; j++) {
            scene = document.createElement("div");
            scene.className = "scene";

            card = document.createElement("div");
            card.id = "card_" + i + "_" + j;
            card.className = "card";
            card.classList.toggle("is-flipped");
            card.onclick = ((py, px) => function(){
                select(py, px);
            })(i, j);

            card_front = document.createElement("div");
            card_front.className = "card_face card_front";
            card_front.style.backgroundImage = "url(" + game.card_set + "/" + game.field[i][j].number + ".jpg)";

            card_back = document.createElement("div");
            card_back.className = "card_face card_back";

            card.appendChild(card_back);            
            card.appendChild(card_front);
            scene.appendChild(card);
            row.appendChild(scene);
        }

        area.appendChild(row);
    }
}

function invalidateScore(){
    for (var i = 0 ; i < game.players.length ; i++)
        document.getElementById("player_score_" + i).textContent = game.players[i].score;
}

function invalidateTurn(){
    document.getElementById("turn_info").textContent = game.players[game.turn.player].name;
}

function createField(width, heigth) {
    var count = width * heigth;
    var x, y;

    var f = Array(heigth).fill().map(() => Array(width));

    for (var i = 0 ; i < count ; ){
        x = Math.floor(Math.random() * width);
        y = Math.floor(Math.random() * heigth);

        if (f[y][x] != undefined)
            continue;
        
        f[y][x] = {
            number: Math.floor(i/2),
            flipped: false,
            found: false
        }

        i++;
    }

    return f;
}

function flip(y, x, value) {
    game.field[y][x].flipped = value;
    document.getElementById("card_" + y + "_" + x).classList.toggle("is-flipped", !value);
}

function select(y, x) {
    checkSelection(y, x);
    flip(y, x, true);

    if (game.turn.first_selection == undefined)
        game.turn.first_selection = [y,x];
    else 
        game.turn.second_selection = [y,x];

    if (isTurnFinished())
        processTurn();
}

function checkSelection(y, x){
    if (isTurnFinished())
        throw new Error("The turn is finished");
    if (game.field[y][x].found)
        throw new Error("This card has already been found.");
    if (game.field[y][x].flipped)
        throw new Error("Please select another card.");
}

function checkTurnFinished(){
    if (!isTurnFinished())
        throw new Error("The turn is not finished.");
}

function isTurnFinished(){
    return game.turn.second_selection != undefined;
}

function processTurn() {
    if (!isTurnFinished())
        throw new Error("The turn is not finished.");

    if (pairFound()){
        var fs = game.turn.first_selection;
        var ss = game.turn.second_selection;
        game.field[fs[0]][fs[1]].found = true;
        game.field[ss[0]][ss[1]].found = true;
        game.players[game.turn.player].score += 2;
        invalidateScore();
        nextTurn();
    }
    else
        setTimeout(nextTurn, 1000);
}

function nextTurn(){
    if (!isTurnFinished())
        throw new Error("The turn is not finished.");

    var fs = game.turn.first_selection;
    var ss = game.turn.second_selection;

    var found = game.field[fs[0]][fs[1]].found && 
                game.field[ss[0]][ss[1]].found;

    if (!found) { 
        flip(fs[0], fs[1], false);
        flip(ss[0], ss[1], false);
        game.turn.player += 1;
        game.turn.player %= game.players.length;
        invalidateTurn();
    }

    game.turn.first_selection = undefined;
    game.turn.second_selection = undefined;
}

function pairFound(){
    var fs = game.turn.first_selection;
    var ss = game.turn.second_selection;
    
    if (fs == undefined || ss == undefined)
        return false;
    
    var fsn = game.field[fs[0]][fs[1]].number;
    var ssn = game.field[ss[0]][ss[1]].number;

    return fsn == ssn;
}