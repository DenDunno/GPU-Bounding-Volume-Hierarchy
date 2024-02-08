
struct Circle2D
{
    float2 position;
    float radius;
};

Circle2D CreateCircle(float2 position, float radius)
{
    Circle2D circle;

    circle.radius = radius;
    circle.position = position;

    return circle;
}

bool IsInside(Circle2D circle, float2 position)
{
    float2 diff = circle.position - position;

    return (diff.x * diff.x + diff.y * diff.y) < (circle.radius * circle.radius);
}