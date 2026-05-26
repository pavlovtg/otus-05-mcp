import json
import os
import pytest
import httpx


MCP_URL = os.environ.get("MCP_URL", "http://localhost:5200")


@pytest.fixture(scope="session")
def mcp_url() -> str:
    return MCP_URL


@pytest.fixture(scope="session")
def mcp_client(mcp_url: str) -> httpx.Client:
    with httpx.Client(base_url=mcp_url, timeout=10.0) as client:
        yield client


def mcp_request(method: str, params: dict | None = None, request_id: int = 1) -> dict:
    return {
        "jsonrpc": "2.0",
        "id": request_id,
        "method": method,
        "params": params or {},
    }


def parse_sse_response(response: httpx.Response) -> dict:
    """Парсит SSE-ответ и возвращает JSON из строки data:."""
    for line in response.text.splitlines():
        if line.startswith("data: "):
            return json.loads(line[6:])
    raise ValueError(f"No data line in SSE response: {response.text!r}")


def initialize_session(client: httpx.Client) -> str | None:
    """Инициализирует MCP-сессию и возвращает session_id."""
    payload = mcp_request(
        "initialize",
        {
            "protocolVersion": "2024-11-05",
            "capabilities": {},
            "clientInfo": {"name": "e2e-test", "version": "1.0"},
        },
    )
    response = client.post(
        "/",
        json=payload,
        headers={"Accept": "application/json, text/event-stream"},
    )
    response.raise_for_status()
    return response.headers.get("mcp-session-id")
