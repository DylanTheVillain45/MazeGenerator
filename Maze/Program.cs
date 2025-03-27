public class Maze {
    public static List<Room> GetConnectingRooms(Room[,] maze, (int, int) current) {
        List<Room> connectingRooms = new List<Room>();
        (int, int)[] directions = [(1, 0), (0, 1), (-1, 0), (0, -1)];

        foreach ((int, int) direction in directions) {
            int y = current.Item1 + direction.Item1;
            int x = current.Item2 + direction.Item2;
            if (y >= 0 && y < maze.GetLength(0) && x >= 0 && x < maze.GetLength(1)) {
                Room room = maze[y, x];
                if (room.top && room.bottom && room.right && room.left) {
                    connectingRooms.Add(room);
                }
            }
        }

        return connectingRooms;
    }
    

    public static void DepthFirstSearch(Room[,] maze, Stack<(int, int)> stack, int depth) {
        // Console.WriteLine(depth);
        // DisplayMaze(maze);
        if ((depth >= (maze.GetLength(0) * maze.GetLength(1)) - 1) || stack.Count == 0) return;
        (int, int) current = stack.Peek();
        int i = current.Item1;
        int j = current.Item2;


        Room currentRoom = maze[i, j];

        Random random = new Random();

        List<Room> connectingRooms = GetConnectingRooms(maze, current);

        if (connectingRooms.Count == 0) {
            // Console.WriteLine("Backtracking");
            stack.Pop();
            DepthFirstSearch(maze, stack, depth);
            return;
        };

        Room nextRoom = connectingRooms[random.Next(0, connectingRooms.Count)];

        (int, int) roomPos = nextRoom.pos;
        int y = roomPos.Item1;
        int x = roomPos.Item2;

        stack.Push(roomPos);

        if (i - 1 == y) {
            currentRoom.top = false;
            nextRoom.bottom = false;
        } else if (i + 1 == y) {
            currentRoom.bottom = false;
            nextRoom.top = false;
        } else if (j + 1 == x) {
            currentRoom.right = false;
            nextRoom.left = false;
        } else if (j - 1 == x) {
            currentRoom.left = false;
            nextRoom.right = false;
        }

        DepthFirstSearch(maze, stack, depth + 1);
    }


    public static void CreateMaze(int height, int width) {
        Room[,] maze = new Room[height, width];

        for (int i = 0; i < height; i++) {
            for (int j = 0; j < width; j++) {
                maze[i, j] = new Room();
                maze[i, j].pos = (i, j);
            }
        }

        (int, int) startRoom = (0, 0);
        (int, int) endRoom = (height - 1, width - 1);

        Stack<(int, int)> stack = new Stack<(int, int)>();
        stack.Push(startRoom);

        DepthFirstSearch(maze, stack, 0);
        DisplayMaze2(maze);
    }
    public static void DisplayMaze(Room[,] maze) {
        char[,] mazeDisplay = new char[maze.GetLength(0) * 2 + 2, maze.GetLength(1) * 2 + 2];

        for (int i = 0; i < maze.GetLength(0); i++) {
            for (int j = 0; j < maze.GetLength(1); j++) {
                Room room = maze[i, j];
                bool[] doors = [room.top, room.bottom, room.right, room.left];
                for (int y = 0; y < 3; y++) {
                    for (int x = 0; x < 3; x++) {
                        char piece;
                        if (i == 0 && j == 0 && y == 0 & x == 0) piece = '┌';
                        else if (i == maze.GetLength(0) - 1 && j == 0 && y == 2 && x == 0) piece = '└';
                        else if (i == 0 && j == maze.GetLength(1) - 1 && y == 0 && x == 2) piece = '┐';
                        else if (i == maze.GetLength(0) - 1 && j == maze.GetLength(1) - 1 && y == 2 && x == 2) piece = '┘';
                        else if (i == 0 && y == 0 && (x == 0 || x == 2)) piece = '┬';
                        else if (j == 0 && x == 0 && (y == 0 || y == 2)) piece = '├';
                        else if (i == maze.GetLength(0) - 1 && y == 2 && (x == 0 || x == 2)) piece = '┴';
                        else if (j == maze.GetLength(1) - 1 && x == 2 && (y == 0 || y == 2)) piece = '┤';
                        else if (y == 0 && x == 1 && doors[0]) piece = '─';
                        else if (y == 2 && x == 1 && doors[1]) piece = '─';
                        else if (y == 1 && x == 2 && doors[2]) piece = '│';
                        else if (y == 1 && x == 0 && doors[3]) piece = '│';
                        else if ((y == 0 || y == 2) && (x == 0 || x == 2)) piece = '┼';
                        else piece = ' ';
                        mazeDisplay[i * 2 + y, j * 2 + x] = piece;
                    }
                }
            }
        }

        for (int l = 0; l < mazeDisplay.GetLength(0); l++) {
            for (int k = 0; k < mazeDisplay.GetLength(1); k++) {
                Console.Write(mazeDisplay[l, k]);
            }
            Console.WriteLine();
        }
    }

    public static void DisplayMaze2(Room[,] maze) {
    int height = maze.GetLength(0);
    int width = maze.GetLength(1);
    char[,] mazeDisplay = new char[height * 3 + 1, width * 3 + 1];

    // Fill with empty spaces
    for (int i = 0; i < mazeDisplay.GetLength(0); i++) {
        for (int j = 0; j < mazeDisplay.GetLength(1); j++) {
            mazeDisplay[i, j] = ' ';
        }
    }

    for (int i = 0; i < height; i++) {
        for (int j = 0; j < width; j++) {
            Room room = maze[i, j];
            int y = i * 3;
            int x = j * 3;

            // Draw corners
            mazeDisplay[y, x] = '┼'; 
            mazeDisplay[y + 3, x] = '┼'; 
            mazeDisplay[y, x + 3] = '┼'; 
            mazeDisplay[y + 3, x + 3] = '┼'; 

            // Horizontal walls
            if (room.top) {
                for (int dx = 1; dx < 3; dx++) mazeDisplay[y, x + dx] = '─';
            }
            if (room.bottom) {
                for (int dx = 1; dx < 3; dx++) mazeDisplay[y + 3, x + dx] = '─';
            }

            // Vertical walls
            if (room.left) {
                for (int dy = 1; dy < 3; dy++) mazeDisplay[y + dy, x] = '│';
            }
            if (room.right) {
                for (int dy = 1; dy < 3; dy++) mazeDisplay[y + dy, x + 3] = '│';
            }
        }
    }

    // Print the maze
    for (int i = 0; i < mazeDisplay.GetLength(0); i++) {
        for (int j = 0; j < mazeDisplay.GetLength(1); j++) {
            Console.Write(mazeDisplay[i, j]);
        }
        Console.WriteLine();
    }
}

}

public class Room {
    public (int, int) pos;
    public bool top = true, bottom = true, right = true, left = true;
}




class Program {
    public static void Main() {
        Maze.CreateMaze(10, 20);
    }
}

