#include <windows.h>
#include <math.h>

typedef struct Vector2 
{
	int x;
	int y;
} Vector2;

int dist(x1, y1, x2, y2);

int clamp(int value, int max, int min);

int getColorToClosest(int x, int y, Vector2 points[]);

LRESULT CALLBACK WindowProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam);

int WINAPI wWinMain(HINSTANCE hInstance, HINSTANCE pInstance, PWSTR cmd, int showCmd)
{
	const wchar_t CLASS_NAME[] = L"Game";
	const int WIDTH = 600;
	const int HEIGHT = 600;
	BOOL running = TRUE;
	
	WNDCLASS wc = { 0 };
	wc.lpszClassName = CLASS_NAME;
	wc.lpfnWndProc = WindowProc;
	wc.hInstance = hInstance;

	RegisterClass(&wc);

	RECT wr = { 0, 0, WIDTH, HEIGHT };
	AdjustWindowRect(&wr, WS_OVERLAPPEDWINDOW, FALSE);

	HWND hwnd;
	hwnd = CreateWindow(CLASS_NAME, CLASS_NAME, WS_OVERLAPPEDWINDOW | WS_VISIBLE, CW_USEDEFAULT, CW_USEDEFAULT, wr.right - wr.left, wr.bottom - wr.top, 0, 0, hInstance, 0);
	if (hwnd == NULL) return 0;

	ShowWindow(hwnd, showCmd);

	Vector2 points[2] = { {(int)WIDTH/2, (int)HEIGHT/2}, {(int)WIDTH/5, (int)HEIGHT/1.2} };

	PAINTSTRUCT ps;
	HDC hdc = BeginPaint(hwnd, &ps);
	for (int x = 0; x < WIDTH; x++)
	{
		for (int y = 0; y < HEIGHT; y++)
		{
			int col = getColorToClosest(x, y, points);
			SetPixel(hdc, x, y, RGB(col, col, col));
		}
	}
	EndPaint(hwnd, &ps);

	MSG msg = {0};

	while (running)
	{
		while (PeekMessage(&msg, 0, 0, 0, PM_REMOVE))
		{
			if (msg.message == WM_QUIT) running = FALSE;
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
	}
	
	return 0;
}

LRESULT CALLBACK WindowProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
	switch (msg)
	{
	case WM_DESTROY:
		PostQuitMessage(0);
		return 0;
	}
	return DefWindowProc(hwnd, msg, wParam, lParam);
}

int dist(int x1, int y1, int x2, int y2)
{
	int x = x2 - x1;
	int y = y2 - y1;
	return (int) sqrt((x * x) + (y * y));
}

int clamp(int d, int min, int max) {
	const int t = d < min ? min : d;
	return t > max ? max : t;
}

int getColorToClosest(int x, int y, Vector2 points[])
{
	int distToClosestPoint = -1;
	for (size_t i = 0; i < 2; i++)
	{
		int _dist = dist(x, y, points[i].x, points[i].y);
		if (distToClosestPoint == -1 || _dist < distToClosestPoint)
		{
			distToClosestPoint = _dist;
		}
	}
	return 255-clamp(distToClosestPoint, 0, 255);
}
