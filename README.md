# ECsharp 🔧

使用 **C#** + **WinRing0** 驱动绕过 Windows Defender，直接写入笔记本 **EC 嵌入式控制器** 的轻量工具  
专为荣耀（Honor）笔记本（如 J8 系列）打造的“荣耀电脑管家”替代品，轻量、无弹窗、无后台常驻！🚀

![C#](https://img.shields.io/badge/C%23-239120?style=flat&logo=c-sharp&logoColor=white)
![Windows](https://img.shields.io/badge/Windows-0078D6?style=flat&logo=windows&logoColor=white)
![WinRing0](https://img.shields.io/badge/WinRing0-LowLevel-orange?style=flat)

## 🔥 为什么要做这个？

官方“荣耀电脑管家”太臃肿了：
- 体积大、常驻内存
- 加密混淆、反调试
- 启动慢、弹窗多
- 后台偷偷干事（隐私担忧）

所以我写了个极简版：开机自动设置 EC 参数（比如电池充电上限），关机/退出时自动恢复原值，全程只在托盘区有个小图标，几乎不占资源  
纯 C# 实现 + WinRing0 驱动  

，欢迎大佬们优化/重构/做成更通用的版本！

## ✨ 核心功能

- 开机自启 → 自动加载驱动 → 写入自定义 EC 寄存器值（电池阈值、键盘灯超时等）
- 退出 → 自动恢复默认值
- 配置全靠 `config.yaml`（支持多条写入、间隔时间、调试模式）
- 系统托盘图标 → 右键退出，无任何弹窗
- 绕过 Windows Defender（使用了带签名的WinRing0驱动）

## 🚀 快速上手

### 1. 准备环境
- Windows 10/11（x64）
- Visual Studio（推荐 2022） + .NET Framework（项目用旧式 .NET Framework）
- 关闭 Windows Defender 或加入排除（驱动加载需要）

### 2. 编译 & 运行
```bash
# 克隆仓库
git clone https://github.com/4evergr8/ECsharp.git
cd ECsharp

# 用 VS 打开 HonorPCManagerisJ8.sln
# 或者直接用 dotnet CLI（如果已迁移到 SDK 风格）
dotnet build

# 运行（需管理员权限加载驱动）
# 先把 WinRing0x64.sys 放到 exe 同目录
HonorPCManagerisJ8.exe
```

### 3. 配置 config.yaml 示例
```yaml
# 是否开机自启（写入注册表）
autoStart: true

# 调试模式（控制台输出日志）
debug: false

# 写入间隔（毫秒），防止 EC 忙不过来
timeout: 500

# 每次写入后等待（毫秒）
wait: 100

# 开机时写入的寄存器值（地址: 值，十六进制）
startup:
  - address: 0x92  # 电池充电上限低字节（示例：80% → 0x50）
    value: 0x50
  - address: 0x93  # 高字节，通常 0x00
    value: 0x00
  - address: 0xA0  # 键盘背光超时（秒）
    value: 0x1E    # 30 秒

# 退出时恢复的值（可以和 startup 相反）
exit:
  - address: 0x92
    value: 0x64    # 恢复 100%
  - address: 0x93
    value: 0x00
  - address: 0xA0
    value: 0x00    # 永不熄灭背光
```

**注意**：不同机型 EC 寄存器地址完全不同！请用 RWEverything 或其他工具先 dump 确认。  
荣耀MagicBook14 R5500U版的电池阈值通常在 0x92/0x93，键盘灯在 0xA0 左右。

## ⚠️ 注意事项 & 风险

- **硬件风险**：乱写 EC 寄存器可能导致主板 brick、电池损坏等！**风险自负**。
- **仅限特定机型**：目前针对荣耀MagicBook14 R5500U版测试通过，其他机型需自行适配地址。
- **权限**：首次运行需管理员权限加载驱动，后续自启也需。
- 无任何 UI 设置，全靠改 yaml 重启生效。

## ❤️ 感谢 & 灵感来源

- WinRing0 项目（经典低级硬件访问驱动）
- RWEverything（EC 寄存器调试神器）
- NBFC项目(笔记本风扇控制器)

如果这个小工具帮到你，点个 **Star** 支持一下吧～ ⭐✨  
