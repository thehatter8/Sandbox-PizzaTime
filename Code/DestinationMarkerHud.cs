using Sandbox;
using Sandbox.Rendering;
using System;

public sealed class DeliveryOverlay : Component
{
    [Property] public WebSlingerGameplay Gameplay { get; set; }
    [Property] public float EdgePadding { get; set; } = 48f;

    protected override void OnUpdate()
    {
        if (Gameplay is null) return;
        if (!Gameplay.IsStarted) return;

        var cam = Scene.Camera;
        if (cam is null) return;

        var hud = cam.Hud;

        // --- DRAW PIZZA MARKER ---
        if (Gameplay.HasPizzaSpawned && !Gameplay.PlayerHasPizza)
        {
            Vector3 pizzaWorld = Gameplay.PizzaLocation;
            DrawOnScreenMarker(hud, cam, pizzaWorld, "Pizza");
        }

        // --- DRAW DESTINATION MARKER ---
        if (Gameplay.HasDestinationSpawned)
        {
            Vector3 destWorld = Gameplay.CurrentDestination;
            string destName = Gameplay.CurrentDestinationName;
            DrawOnScreenMarker(hud, cam, destWorld, destName);
        }
    }

    private void DrawOnScreenMarker(HudPainter hud, CameraComponent cam, Vector3 worldPos, string label)
    {
        float sw = Screen.Width;
        float sh = Screen.Height;

        var toCam = worldPos - cam.Transform.Position;
        bool isBehind = Vector3.Dot(toCam, cam.Transform.Rotation.Forward) <= 0f;

        var screenNormal = cam.PointToScreenNormal(worldPos);
        var screenPos = new Vector2(screenNormal.x * sw, screenNormal.y * sh);

        bool onScreen = !isBehind
                        && screenPos.x >= 0f && screenPos.x <= sw
                        && screenPos.y >= 0f && screenPos.y <= sh;

        if (onScreen)
        {
            float pulse = MathF.Sin(Time.Now * 3f) * 0.12f + 1f;
            float size = 18f * pulse;

            DrawDiamond(hud, screenPos + new Vector2(2, 2), size, Color.Black.WithAlpha(0.45f));
            DrawDiamond(hud, screenPos, size, new Color(1f, 0f, 0f));
            DrawDiamondOutline(hud, screenPos, size + 3f, Color.White);

            if (Gameplay.Warmup?.PizzaPlayer?.IsValid() == true)
            {
                float dist = Vector3.DistanceBetween(Gameplay.Warmup.PizzaPlayer.GameObject.WorldPosition, worldPos);
                string distTx = $"{dist / 39.37f:0}m";
                hud.DrawText(new TextRendering.Scope(distTx, Color.White.WithAlpha(0.9f), 14),
                             screenPos + new Vector2(0, size + 6),
                             TextFlag.Center | TextFlag.Top);
            }

            hud.DrawText(new TextRendering.Scope(label, new Color(1f, 0.85f, 0.3f), 18),
                         screenPos + new Vector2(0, size + 22),
                         TextFlag.Center | TextFlag.Top);
        }
        else
        {
            DrawOffScreenArrow(hud, screenPos, isBehind, sw, sh, label);
        }
    }

    private void DrawOffScreenArrow(HudPainter hud, Vector2 rawPos, bool behind, float sw, float sh, string label)
    {
        if (behind) rawPos = new Vector2(sw - rawPos.x, sh - rawPos.y);

        var centre = new Vector2(sw * 0.5f, sh * 0.5f);
        var dir = (rawPos - centre).Normal;
        if (dir.IsNearlyZero()) dir = Vector2.Right;

        float angle = MathF.Atan2(dir.y, dir.x);
        var clamped = ClampToScreenEdge(rawPos, sw, sh, EdgePadding);

        DrawArrow(hud, clamped, angle, 20f, Color.Black.WithAlpha(0.45f), offset: 2f);
        DrawArrow(hud, clamped, angle, 20f, new Color(1f, 0.55f, 0f));
        DrawArrow(hud, clamped, angle, 23f, Color.White.WithAlpha(0.5f), outline: true);

        var labelPos = clamped - dir * 30f;
        hud.DrawText(new TextRendering.Scope(label, Color.White, 15),
                     labelPos, TextFlag.Center | TextFlag.Center);
    }

    // --- Helpers ---
    private static void DrawDiamond(HudPainter hud, Vector2 pos, float half, Color color)
    {
        hud.DrawRect(new Rect(pos.x - half * 0.7f, pos.y - half * 0.7f,
                              half * 1.4f, half * 1.4f), color);
    }

    private static void DrawDiamondOutline(HudPainter hud, Vector2 pos, float half, Color color)
    {
        var top = pos + new Vector2(0, -half);
        var right = pos + new Vector2(half, 0);
        var bottom = pos + new Vector2(0, half);
        var left = pos + new Vector2(-half, 0);

        hud.DrawLine(top, right, 1.5f, color);
        hud.DrawLine(right, bottom, 1.5f, color);
        hud.DrawLine(bottom, left, 1.5f, color);
        hud.DrawLine(left, top, 1.5f, color);
    }

    private static void DrawArrow(HudPainter hud, Vector2 pos, float angle, float size, Color color,
                                  float offset = 0f, bool outline = false)
    {
        var tip = pos + new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * (size + offset);
        var bl = pos + new Vector2(MathF.Cos(angle + MathF.PI * 0.75f), MathF.Sin(angle + MathF.PI * 0.75f)) * (size * 0.6f + offset);
        var br = pos + new Vector2(MathF.Cos(angle - MathF.PI * 0.75f), MathF.Sin(angle - MathF.PI * 0.75f)) * (size * 0.6f + offset);

        float thickness = outline ? 1.5f : 2.5f;
        hud.DrawLine(tip, bl, thickness, color);
        hud.DrawLine(bl, br, thickness, color);
        hud.DrawLine(br, tip, thickness, color);

        if (!outline)
        {
            for (int i = 0; i <= 6; i++)
                hud.DrawLine(tip, Vector2.Lerp(bl, br, i / 6f), 1.5f, color);
        }
    }

    private static Vector2 ClampToScreenEdge(Vector2 pos, float sw, float sh, float pad)
    {
        var centre = new Vector2(sw * 0.5f, sh * 0.5f);
        var dir = pos - centre;
        float minX = pad, maxX = sw - pad;
        float minY = pad, maxY = sh - pad;
        float tMin = float.MaxValue;

        if (MathF.Abs(dir.x) > 0.001f)
        {
            float t = dir.x > 0 ? (maxX - centre.x) / dir.x : (minX - centre.x) / dir.x;
            tMin = MathF.Min(tMin, t);
        }
        if (MathF.Abs(dir.y) > 0.001f)
        {
            float t = dir.y > 0 ? (maxY - centre.y) / dir.y : (minY - centre.y) / dir.y;
            tMin = MathF.Min(tMin, t);
        }

        return centre + dir * MathF.Min(tMin, 1f);
    }
}