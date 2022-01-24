using System;
using System.Collections.Generic;

public static class PathFinder
{
    public static List<Tile> FindPath_AStar(TileGrid grid, Tile start, Tile end, List<IVisualStep> outSteps)
    { 
         // hiển thị tiến trình
         outSteps.Add(new MarkStartTileStep(start));
         outSteps.Add(new MarkEndTileStep(end));

        // cài đặt lại chi phí toàn bảng
        foreach (var tile in grid.Tiles)
        {
            tile.Cost = int.MaxValue;
        }

        start.Cost = 0; // đặt chi phí ô bắt đầu =0

        // so sánh heuristic, tính chi phí của đường dẫn từ lhs, rhs đến mục tiêu(end)
        Comparison<Tile> heuristicComparison = (lhs, rhs) =>
        {
            float lhsCost = lhs.Cost + GetEuclideanHeuristicCost(lhs, end);
            float rhsCost = rhs.Cost + GetEuclideanHeuristicCost(rhs, end);

            return lhsCost.CompareTo(rhsCost);
        };

        // minheap lưu trữ các đơn vị liền kề của 1 Tile(theo 4 phương, hoặc tám hướng)
        MinHeap<Tile> frontier = new MinHeap<Tile>(heuristicComparison);
        frontier.Add(start);

        // hashset lưu trữ các tile đã được duyệt
        HashSet<Tile> visited = new HashSet<Tile>();
        visited.Add(start);

        start.PrevTile = null;

        while (frontier.Count > 0) // lấy tile từ từng hướng của Tile đang duyệt
        {
            Tile current = frontier.Remove();

            // hiển thị tiến trình
            if (Values.SHOW_STEPS)
            if (current != start && current != end)
            {
                outSteps.Add(new VisitTileStep(current));
            }

            if (current == end)
            {
                break;
            }

            foreach (var neighbor in grid.GetNeighbors(current))
            {
                int newNeighborCost = current.Cost + neighbor.Weight; 
                // xác định tile có chi phí đến mục tiêu rẻ nhất(đường =1. tường = 100)
                if (newNeighborCost < neighbor.Cost) // đúng trong trường hợp neighbor là Tile chưa được duyệt
                {
                    neighbor.Cost = newNeighborCost; // trả lại chi phí cho neighbor
                    neighbor.PrevTile = current;    // lưu Tile trước đó (để truy vết)
                }

                if (!visited.Contains(neighbor)) // nếu chưa được duyệt/lưu -> tiến hành lưu
                {
                    frontier.Add(neighbor);
                    visited.Add(neighbor);

                    if (Values.SHOW_STEPS)
                    // hiển thị tiến trình
                    if (neighbor != end)
                    {
                        outSteps.Add(new PushTileInFrontierStep(neighbor, neighbor.Cost));
                    }
                
                }
            }
        }

        // truy ngược từ end 
        List<Tile> path = BacktrackToPath(end);

        if (Values.SHOW_PATH)
        // hiển thị tiến trình
        foreach (var tile in path)
        {
            if (tile == start || tile == end)
            {
                continue;
            }

            outSteps.Add(new MarkPathTileStep(tile));
        }

        return path;
    }

    private static float GetEuclideanHeuristicCost(Tile current, Tile end)
    {
        float heuristicCost = (current.ToVector2() - end.ToVector2()).magnitude;
        return heuristicCost;
    }

    private static List<Tile> BacktrackToPath(Tile end)
    {
        Tile current = end;
        List<Tile> path = new List<Tile>();

        while (current != null)
        {
            path.Add(current);
            current = current.PrevTile;
        }

        path.Reverse();

        return path;
    }
}

public interface IVisualStep
{
    void Execute();
}

public abstract class VisualStep : IVisualStep
{
    protected Tile _tile;

    public VisualStep(Tile tile)
    {
        _tile = tile;
    }

    public abstract void Execute();
}

public class MarkStartTileStep : VisualStep
{
    public MarkStartTileStep(Tile tile) : base(tile)
    {
    }

    public override void Execute()
    {
        _tile.SetColor(_tile.Grid.TileColor_Start);
    }
}

public class MarkEndTileStep : VisualStep
{
    public MarkEndTileStep(Tile tile) : base(tile)
    {
    }

    public override void Execute()
    {
        _tile.SetColor(_tile.Grid.TileColor_End);
    }
}

public class MarkPathTileStep : VisualStep
{
    public MarkPathTileStep(Tile tile) : base(tile)
    {
    }

    public override void Execute()
    {
        _tile.SetColor(_tile.Grid.TileColor_Path);
    }
}

public class PushTileInFrontierStep : VisualStep
{
    private int _cost;

    public PushTileInFrontierStep(Tile tile, int cost) : base(tile)
    {
        _cost = cost;
    }

    public override void Execute()
    {
        _tile.SetColor(_tile.Grid.TileColor_Frontier);
        _tile.SetText(_cost != 0 ? _cost.ToString() : "");
    }
}

public class VisitTileStep : VisualStep
{
    public VisitTileStep(Tile tile) : base(tile)
    {
    }

    public override void Execute()
    {
        _tile.SetColor(_tile.Grid.TileColor_Visited);
    }
}